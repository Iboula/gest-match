namespace GestMatch.Domain.Enums;

/// <summary>
/// Statut d'un billet
/// </summary>
public enum TicketStatus
{
    /// <summary>
    /// Billet valide, non encore utilisé
    /// </summary>
    Valid = 0,
    
    /// <summary>
    /// Billet déjà utilisé (scanné à l'entrée)
    /// </summary>
    Used = 1,
    
    /// <summary>
    /// Billet annulé (match annulé ou remboursement)
    /// </summary>
    Cancelled = 2,
    
    /// <summary>
    /// Billet expiré
    /// </summary>
    Expired = 3
}
