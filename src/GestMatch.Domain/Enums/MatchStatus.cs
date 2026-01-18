namespace GestMatch.Domain.Enums;

/// <summary>
/// Statut d'un match
/// </summary>
public enum MatchStatus
{
    /// <summary>
    /// Match programmé, en attente
    /// </summary>
    Scheduled = 0,
    
    /// <summary>
    /// Match en cours
    /// </summary>
    InProgress = 1,
    
    /// <summary>
    /// Match terminé
    /// </summary>
    Finished = 2,
    
    /// <summary>
    /// Match reporté à une date ultérieure
    /// </summary>
    Postponed = 3,
    
    /// <summary>
    /// Match annulé
    /// </summary>
    Cancelled = 4
}
