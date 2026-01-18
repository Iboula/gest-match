using System.Security.Claims;
using GestMatch.Application.DTOs.Tickets;
using GestMatch.Application.Interfaces;

namespace GestMatch.Api.Extensions;

/// <summary>
/// Extensions pour les endpoints de gestion des billets
/// </summary>
public static class TicketEndpoints
{
    public static void MapTicketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tickets")
            .WithTags("Tickets")
            .WithOpenApi();

        // GET /api/tickets/{id} - Détails d'un billet
        group.MapGet("/{id:guid}", async (
            Guid id,
            ITicketService ticketService,
            CancellationToken cancellationToken) =>
        {
            var ticket = await ticketService.GetTicketByIdAsync(id, cancellationToken);
            return ticket != null ? Results.Ok(ticket) : Results.NotFound();
        })
        .WithName("GetTicketById")
        .WithSummary("Obtenir les détails d'un billet")
        .RequireAuthorization()
        .Produces<TicketResponse>()
        .Produces(StatusCodes.Status404NotFound);

        // GET /api/tickets/user/me - Mes billets
        group.MapGet("/user/me", async (
            ITicketService ticketService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var userId = GetUserId(user);
            var tickets = await ticketService.GetUserTicketsAsync(userId, cancellationToken);
            return Results.Ok(tickets);
        })
        .WithName("GetMyTickets")
        .WithSummary("Obtenir mes billets")
        .RequireAuthorization()
        .Produces<IEnumerable<TicketResponse>>();

        // GET /api/tickets/match/{matchId} - Billets d'un match
        group.MapGet("/match/{matchId:guid}", async (
            Guid matchId,
            ITicketService ticketService,
            CancellationToken cancellationToken) =>
        {
            var tickets = await ticketService.GetMatchTicketsAsync(matchId, cancellationToken);
            return Results.Ok(tickets);
        })
        .WithName("GetMatchTickets")
        .WithSummary("Obtenir les billets vendus pour un match")
        .RequireAuthorization("RequireMatchManager")
        .Produces<IEnumerable<TicketResponse>>();

        // POST /api/tickets/purchase - Acheter un billet
        group.MapPost("/purchase", async (
            PurchaseTicketRequest request,
            ITicketService ticketService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var userId = GetUserId(user);
            
            try
            {
                var ticket = await ticketService.PurchaseTicketAsync(request, userId, cancellationToken);
                return Results.Created($"/api/tickets/{ticket.Id}", ticket);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        })
        .WithName("PurchaseTicket")
        .WithSummary("Acheter un billet pour un match")
        .RequireAuthorization()
        .Produces<TicketResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        // POST /api/tickets/scan - Scanner un billet (pour l'entrée)
        group.MapPost("/scan", async (
            ScanTicketRequest request,
            ITicketService ticketService,
            CancellationToken cancellationToken) =>
        {
            var result = await ticketService.ScanTicketAsync(request, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("ScanTicket")
        .WithSummary("Scanner un billet à l'entrée du stade")
        .RequireAuthorization("RequireMatchManager")
        .Produces<ScanTicketResponse>();

        // DELETE /api/tickets/{id} - Annuler un billet
        group.MapDelete("/{id:guid}", async (
            Guid id,
            ITicketService ticketService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var userId = GetUserId(user);
            
            try
            {
                await ticketService.CancelTicketAsync(id, userId, cancellationToken);
                return Results.NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        })
        .WithName("CancelTicket")
        .WithSummary("Annuler un billet")
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden);
    }

    private static Guid GetUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.FindFirst("sub")?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in claims");

        return Guid.TryParse(userIdClaim, out var userId) ? userId : throw new UnauthorizedAccessException("Invalid user ID");
    }
}
