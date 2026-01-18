namespace GestMatch.Domain.Enums;

/// <summary>
/// Type de billet
/// </summary>
public enum TicketType
{
    /// <summary>
    /// Billet standard
    /// </summary>
    Standard = 0,
    
    /// <summary>
    /// Billet VIP
    /// </summary>
    VIP = 1,
    
    /// <summary>
    /// Billet gratuit (matchs communautaires)
    /// </summary>
    Free = 2
}
