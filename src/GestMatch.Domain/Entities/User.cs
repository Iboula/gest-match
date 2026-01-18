using GestMatch.Domain.Common;
using GestMatch.Domain.Enums;

namespace GestMatch.Domain.Entities;

/// <summary>
/// Entité utilisateur synchronisée avec Zitadel
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Identifiant de l'utilisateur dans Zitadel (sub claim)
    /// </summary>
    public required string ZitadelId { get; set; }
    
    /// <summary>
    /// Email de l'utilisateur
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Nom complet de l'utilisateur
    /// </summary>
    public required string FullName { get; set; }
    
    /// <summary>
    /// Numéro de téléphone (important pour le contexte sénégalais)
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Rôle de l'utilisateur dans le système
    /// </summary>
    public UserRole Role { get; set; } = UserRole.User;
    
    /// <summary>
    /// Indique si le compte est actif
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Ville de l'utilisateur
    /// </summary>
    public string? City { get; set; }
    
    // Navigation properties
    
    /// <summary>
    /// Matchs créés par ce gestionnaire (si MatchManager ou Admin)
    /// </summary>
    public ICollection<Match> CreatedMatches { get; set; } = new List<Match>();
    
    /// <summary>
    /// Billets achetés par cet utilisateur
    /// </summary>
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    /// <summary>
    /// Paiements effectués par cet utilisateur
    /// </summary>
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
