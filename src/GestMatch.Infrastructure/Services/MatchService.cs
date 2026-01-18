using GestMatch.Application.DTOs.Matches;
using GestMatch.Application.Interfaces;
using GestMatch.Domain.Entities;
using GestMatch.Domain.Enums;
using GestMatch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestMatch.Infrastructure.Services;

/// <summary>
/// Implémentation du service de gestion des matchs
/// </summary>
public class MatchService : IMatchService
{
    private readonly ApplicationDbContext _context;

    public MatchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MatchResponse> CreateMatchAsync(CreateMatchRequest request, Guid createdByUserId, CancellationToken cancellationToken = default)
    {
        var match = new Match
        {
            TeamA = request.TeamA,
            TeamB = request.TeamB,
            MatchDateTime = request.MatchDateTime,
            Stadium = request.Stadium,
            City = request.City,
            Region = request.Region,
            Competition = request.Competition,
            MatchType = request.MatchType,
            Description = request.Description,
            PosterUrl = request.PosterUrl,
            StandardTicketPrice = request.StandardTicketPrice,
            VipTicketPrice = request.VipTicketPrice,
            TotalTicketsAvailable = request.TotalTicketsAvailable,
            VipTicketsAvailable = request.VipTicketsAvailable,
            TicketSaleEndDate = request.TicketSaleEndDate,
            CreatedByUserId = createdByUserId,
            Status = MatchStatus.Scheduled
        };

        _context.Matches.Add(match);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetMatchResponseAsync(match.Id, cancellationToken);
    }

    public async Task<MatchResponse> UpdateMatchAsync(Guid matchId, UpdateMatchRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var match = await _context.Matches
            .FirstOrDefaultAsync(m => m.Id == matchId, cancellationToken)
            ?? throw new InvalidOperationException($"Match {matchId} not found");

        // Vérifier que l'utilisateur est le créateur du match
        if (match.CreatedByUserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this match");
        }

        // Mettre à jour uniquement les champs fournis
        if (request.TeamA != null) match.TeamA = request.TeamA;
        if (request.TeamB != null) match.TeamB = request.TeamB;
        if (request.MatchDateTime.HasValue) match.MatchDateTime = request.MatchDateTime.Value;
        if (request.Stadium != null) match.Stadium = request.Stadium;
        if (request.City != null) match.City = request.City;
        if (request.Region != null) match.Region = request.Region;
        if (request.Competition != null) match.Competition = request.Competition;
        if (request.MatchType.HasValue) match.MatchType = request.MatchType.Value;
        if (request.Status.HasValue) match.Status = request.Status.Value;
        if (request.Description != null) match.Description = request.Description;
        if (request.PosterUrl != null) match.PosterUrl = request.PosterUrl;
        if (request.StandardTicketPrice.HasValue) match.StandardTicketPrice = request.StandardTicketPrice.Value;
        if (request.VipTicketPrice.HasValue) match.VipTicketPrice = request.VipTicketPrice;
        if (request.TotalTicketsAvailable.HasValue) match.TotalTicketsAvailable = request.TotalTicketsAvailable.Value;
        if (request.VipTicketsAvailable.HasValue) match.VipTicketsAvailable = request.VipTicketsAvailable;
        if (request.TicketSaleEndDate.HasValue) match.TicketSaleEndDate = request.TicketSaleEndDate;
        if (request.ScoreTeamA.HasValue) match.ScoreTeamA = request.ScoreTeamA;
        if (request.ScoreTeamB.HasValue) match.ScoreTeamB = request.ScoreTeamB;

        match.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return await GetMatchResponseAsync(match.Id, cancellationToken);
    }

    public async Task DeleteMatchAsync(Guid matchId, Guid userId, CancellationToken cancellationToken = default)
    {
        var match = await _context.Matches
            .FirstOrDefaultAsync(m => m.Id == matchId, cancellationToken)
            ?? throw new InvalidOperationException($"Match {matchId} not found");

        if (match.CreatedByUserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this match");
        }

        _context.Matches.Remove(match);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<MatchResponse?> GetMatchByIdAsync(Guid matchId, CancellationToken cancellationToken = default)
    {
        return await GetMatchResponseAsync(matchId, cancellationToken);
    }

    public async Task<IEnumerable<MatchSummaryResponse>> GetMatchesAsync(
        string? city = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        MatchStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Matches.AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(m => m.City.ToLower().Contains(city.ToLower()));
        }

        if (fromDate.HasValue)
        {
            query = query.Where(m => m.MatchDateTime >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(m => m.MatchDateTime <= toDate.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(m => m.Status == status.Value);
        }

        var matches = await query
            .OrderBy(m => m.MatchDateTime)
            .Select(m => new MatchSummaryResponse(
                m.Id,
                m.TeamA,
                m.TeamB,
                m.MatchDateTime,
                m.Stadium,
                m.City,
                m.Competition,
                m.MatchType,
                m.Status,
                m.StandardTicketPrice,
                m.TotalTicketsAvailable - m.Tickets.Count(t => t.Status == TicketStatus.Valid || t.Status == TicketStatus.Used),
                m.PosterUrl
            ))
            .ToListAsync(cancellationToken);

        return matches;
    }

    public async Task<IEnumerable<MatchSummaryResponse>> GetMatchesByManagerAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        var matches = await _context.Matches
            .Where(m => m.CreatedByUserId == managerId)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MatchSummaryResponse(
                m.Id,
                m.TeamA,
                m.TeamB,
                m.MatchDateTime,
                m.Stadium,
                m.City,
                m.Competition,
                m.MatchType,
                m.Status,
                m.StandardTicketPrice,
                m.TotalTicketsAvailable - m.Tickets.Count(t => t.Status == TicketStatus.Valid || t.Status == TicketStatus.Used),
                m.PosterUrl
            ))
            .ToListAsync(cancellationToken);

        return matches;
    }

    // Méthode helper privée
    private async Task<MatchResponse> GetMatchResponseAsync(Guid matchId, CancellationToken cancellationToken)
    {
        var match = await _context.Matches
            .Include(m => m.CreatedByUser)
            .Include(m => m.Tickets)
            .FirstOrDefaultAsync(m => m.Id == matchId, cancellationToken)
            ?? throw new InvalidOperationException($"Match {matchId} not found");

        var ticketsSold = match.Tickets.Count(t => t.Status == TicketStatus.Valid || t.Status == TicketStatus.Used);

        return new MatchResponse(
            match.Id,
            match.TeamA,
            match.TeamB,
            match.MatchDateTime,
            match.Stadium,
            match.City,
            match.Region,
            match.Competition,
            match.MatchType,
            match.Status,
            match.Description,
            match.PosterUrl,
            match.StandardTicketPrice,
            match.VipTicketPrice,
            match.TotalTicketsAvailable,
            match.VipTicketsAvailable,
            ticketsSold,
            match.TotalTicketsAvailable - ticketsSold,
            match.TicketSaleEndDate,
            match.ScoreTeamA,
            match.ScoreTeamB,
            match.CreatedByUserId,
            match.CreatedByUser?.FullName ?? "Unknown",
            match.CreatedAt,
            match.UpdatedAt
        );
    }
}
