using GestMatch.Domain.Enums;

namespace GestMatch.Application.DTOs.Matches;

/// <summary>
/// DTO pour la création d'un match
/// </summary>
public record CreateMatchRequest(
    string TeamA,
    string TeamB,
    DateTime MatchDateTime,
    string Stadium,
    string City,
    string? Region,
    string Competition,
    GestMatch.Domain.Enums.MatchType MatchType,
    string? Description,
    string? PosterUrl,
    decimal StandardTicketPrice,
    decimal? VipTicketPrice,
    int TotalTicketsAvailable,
    int? VipTicketsAvailable,
    DateTime? TicketSaleEndDate
);

/// <summary>
/// DTO pour la mise à jour d'un match
/// </summary>
public record UpdateMatchRequest(
    string? TeamA,
    string? TeamB,
    DateTime? MatchDateTime,
    string? Stadium,
    string? City,
    string? Region,
    string? Competition,
    GestMatch.Domain.Enums.MatchType? MatchType,
    MatchStatus? Status,
    string? Description,
    string? PosterUrl,
    decimal? StandardTicketPrice,
    decimal? VipTicketPrice,
    int? TotalTicketsAvailable,
    int? VipTicketsAvailable,
    DateTime? TicketSaleEndDate,
    int? ScoreTeamA,
    int? ScoreTeamB
);

/// <summary>
/// DTO pour la réponse d'un match
/// </summary>
public record MatchResponse(
    Guid Id,
    string TeamA,
    string TeamB,
    DateTime MatchDateTime,
    string Stadium,
    string City,
    string? Region,
    string Competition,
    GestMatch.Domain.Enums.MatchType MatchType,
    MatchStatus Status,
    string? Description,
    string? PosterUrl,
    decimal StandardTicketPrice,
    decimal? VipTicketPrice,
    int TotalTicketsAvailable,
    int? VipTicketsAvailable,
    int TicketsSold,
    int TicketsRemaining,
    DateTime? TicketSaleEndDate,
    int? ScoreTeamA,
    int? ScoreTeamB,
    Guid CreatedByUserId,
    string CreatedByUserName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

/// <summary>
/// DTO pour le résumé d'un match (liste)
/// </summary>
public record MatchSummaryResponse(
    Guid Id,
    string TeamA,
    string TeamB,
    DateTime MatchDateTime,
    string Stadium,
    string City,
    string Competition,
    GestMatch.Domain.Enums.MatchType MatchType,
    MatchStatus Status,
    decimal StandardTicketPrice,
    int TicketsRemaining,
    string? PosterUrl
);
