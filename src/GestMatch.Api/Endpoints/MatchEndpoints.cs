using System.Security.Claims;
using GestMatch.Application.DTOs.Matches;
using GestMatch.Application.Interfaces;
using GestMatch.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GestMatch.Api.Extensions;

/// <summary>
/// Extensions pour les endpoints de gestion des matchs
/// </summary>
public static class MatchEndpoints
{
    public static void MapMatchEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/matches")
            .WithTags("Matches")
            .WithOpenApi();

        // GET /api/matches - Liste des matchs (public)
        group.MapGet("/", async (
            IMatchService matchService,
            [FromQuery] string? city,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] MatchStatus? status,
            CancellationToken cancellationToken) =>
        {
            var matches = await matchService.GetMatchesAsync(city, fromDate, toDate, status, cancellationToken);
            return Results.Ok(matches);
        })
        .WithName("GetMatches")
        .WithSummary("Obtenir la liste des matchs avec filtres optionnels")
        .Produces<IEnumerable<MatchSummaryResponse>>();

        // GET /api/matches/{id} - Détails d'un match (public)
        group.MapGet("/{id:guid}", async (
            Guid id,
            IMatchService matchService,
            CancellationToken cancellationToken) =>
        {
            var match = await matchService.GetMatchByIdAsync(id, cancellationToken);
            return match != null ? Results.Ok(match) : Results.NotFound();
        })
        .WithName("GetMatchById")
        .WithSummary("Obtenir les détails d'un match")
        .Produces<MatchResponse>()
        .Produces(StatusCodes.Status404NotFound);

        // GET /api/matches/manager/{managerId} - Matchs d'un gestionnaire
        group.MapGet("/manager/{managerId:guid}", async (
            Guid managerId,
            IMatchService matchService,
            CancellationToken cancellationToken) =>
        {
            var matches = await matchService.GetMatchesByManagerAsync(managerId, cancellationToken);
            return Results.Ok(matches);
        })
        .WithName("GetMatchesByManager")
        .WithSummary("Obtenir les matchs créés par un gestionnaire")
        .RequireAuthorization("RequireMatchManager")
        .Produces<IEnumerable<MatchSummaryResponse>>();

        // POST /api/matches - Créer un match (MatchManager ou Admin)
        group.MapPost("/", async (
            CreateMatchRequest request,
            IMatchService matchService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var userId = GetUserId(user);
            var match = await matchService.CreateMatchAsync(request, userId, cancellationToken);
            return Results.Created($"/api/matches/{match.Id}", match);
        })
        .WithName("CreateMatch")
        .WithSummary("Créer un nouveau match (MatchManager ou Admin uniquement)")
        .RequireAuthorization("RequireMatchManager")
        .Produces<MatchResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden);

        // PUT /api/matches/{id} - Modifier un match
        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateMatchRequest request,
            IMatchService matchService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var userId = GetUserId(user);
            
            try
            {
                var match = await matchService.UpdateMatchAsync(id, request, userId, cancellationToken);
                return Results.Ok(match);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new { Error = ex.Message });
            }
        })
        .WithName("UpdateMatch")
        .WithSummary("Modifier un match existant")
        .RequireAuthorization("RequireMatchManager")
        .Produces<MatchResponse>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status403Forbidden);

        // DELETE /api/matches/{id} - Supprimer un match
        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMatchService matchService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var userId = GetUserId(user);
            
            try
            {
                await matchService.DeleteMatchAsync(id, userId, cancellationToken);
                return Results.NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new { Error = ex.Message });
            }
        })
        .WithName("DeleteMatch")
        .WithSummary("Supprimer un match")
        .RequireAuthorization("RequireMatchManager")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
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
