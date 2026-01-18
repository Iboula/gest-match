using GestMatch.Application.DTOs.Matches;
using GestMatch.Domain.Enums;

namespace GestMatch.Application.Interfaces;

/// <summary>
/// Service de gestion des matchs
/// </summary>
public interface IMatchService
{
    /// <summary>
    /// Créer un nouveau match (MatchManager ou Admin uniquement)
    /// </summary>
    Task<MatchResponse> CreateMatchAsync(CreateMatchRequest request, Guid createdByUserId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Mettre à jour un match existant
    /// </summary>
    Task<MatchResponse> UpdateMatchAsync(Guid matchId, UpdateMatchRequest request, Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Supprimer un match
    /// </summary>
    Task DeleteMatchAsync(Guid matchId, Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir un match par ID
    /// </summary>
    Task<MatchResponse?> GetMatchByIdAsync(Guid matchId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lister les matchs avec filtres
    /// </summary>
    Task<IEnumerable<MatchSummaryResponse>> GetMatchesAsync(
        string? city = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        MatchStatus? status = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir les matchs créés par un gestionnaire
    /// </summary>
    Task<IEnumerable<MatchSummaryResponse>> GetMatchesByManagerAsync(Guid managerId, CancellationToken cancellationToken = default);
}
