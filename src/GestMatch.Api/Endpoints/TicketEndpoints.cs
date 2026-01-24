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
        .AllowAnonymous()
        .WithName("GetTicketById")
        .WithSummary("Obtenir les détails d'un billet")
        .Produces<TicketResponse>()
        .Produces(StatusCodes.Status404NotFound);

        // GET /api/tickets/user/me - Mes billets
        group.MapGet("/user/me", async (
            ITicketService ticketService,
            CancellationToken cancellationToken) =>
        {
            // UserId fixe pour les tests
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var tickets = await ticketService.GetUserTicketsAsync(userId, cancellationToken);
            return Results.Ok(tickets);
        })
        .AllowAnonymous()
        .WithName("GetMyTickets")
        .WithSummary("Obtenir mes billets")
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
        .AllowAnonymous()
        .WithName("GetMatchTickets")
        .WithSummary("Obtenir les billets vendus pour un match")
        .Produces<IEnumerable<TicketResponse>>();

        // POST /api/tickets/purchase - Acheter un billet
        group.MapPost("/purchase", async (
            PurchaseTicketRequest request,
            ITicketService ticketService,
            CancellationToken cancellationToken) =>
        {
            // UserId fixe pour les tests
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            
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
        .AllowAnonymous()
        .WithName("PurchaseTicket")
        .WithSummary("Acheter un billet pour un match")
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
        .AllowAnonymous()
        .WithName("ScanTicket")
        .WithSummary("Scanner un billet à l'entrée du stade")
        .Produces<ScanTicketResponse>();

        // DELETE /api/tickets/{id} - Annuler un billet
        group.MapDelete("/{id:guid}", async (
            Guid id,
            ITicketService ticketService,
            CancellationToken cancellationToken) =>
        {
            // UserId fixe pour les tests
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            
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
        .AllowAnonymous()
        .WithName("CancelTicket")
        .WithSummary("Annuler un billet")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
