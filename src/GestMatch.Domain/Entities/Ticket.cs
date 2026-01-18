using GestMatch.Domain.Common;
using GestMatch.Domain.Enums;

namespace GestMatch.Domain.Entities;

/// <summary>
/// Entité représentant un billet pour un match
/// </summary>
public class Ticket : BaseEntity
{
    /// <summary>
    /// Numéro unique du billet (lisible par l'utilisateur)
    /// </summary>
    public required string TicketNumber { get; set; }
    
    /// <summary>
    /// Type de billet
    /// </summary>
    public TicketType TicketType { get; set; } = TicketType.Standard;
    
    /// <summary>
    /// Statut actuel du billet
    /// </summary>
    public TicketStatus Status { get; set; } = TicketStatus.Valid;
    
    /// <summary>
    /// Prix payé pour ce billet
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Données du QR Code (contient l'ID du billet + signature)
    /// </summary>
    public required string QrCodeData { get; set; }
    
    /// <summary>
    /// URL de l'image du QR Code (stockée ou générée à la volée)
    /// </summary>
    public string? QrCodeImageUrl { get; set; }
    
    /// <summary>
    /// Date et heure d'utilisation du billet (scan à l'entrée)
    /// </summary>
    public DateTime? UsedAt { get; set; }
    
    /// <summary>
    /// Date et heure d'annulation du billet
    /// </summary>
    public DateTime? CancelledAt { get; set; }
    
    /// <summary>
    /// Nom du détenteur du billet
    /// </summary>
    public string? HolderName { get; set; }
    
    /// <summary>
    /// Numéro de téléphone du détenteur
    /// </summary>
    public string? HolderPhone { get; set; }
    
    // Foreign Keys
    
    /// <summary>
    /// Identifiant du match associé
    /// </summary>
    public Guid MatchId { get; set; }
    
    /// <summary>
    /// Identifiant de l'utilisateur qui a acheté le billet
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Identifiant du paiement associé
    /// </summary>
    public Guid? PaymentId { get; set; }
    
    // Navigation properties
    
    /// <summary>
    /// Match associé à ce billet
    /// </summary>
    public Match? Match { get; set; }
    
    /// <summary>
    /// Utilisateur qui a acheté le billet
    /// </summary>
    public User? User { get; set; }
    
    /// <summary>
    /// Paiement associé à ce billet
    /// </summary>
    public Payment? Payment { get; set; }
}
