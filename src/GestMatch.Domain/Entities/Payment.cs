using GestMatch.Domain.Common;
using GestMatch.Domain.Enums;

namespace GestMatch.Domain.Entities;

/// <summary>
/// Entité représentant un paiement
/// </summary>
public class Payment : BaseEntity
{
    /// <summary>
    /// Référence unique du paiement
    /// </summary>
    public required string PaymentReference { get; set; }
    
    /// <summary>
    /// Montant du paiement
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Méthode de paiement utilisée
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }
    
    /// <summary>
    /// Statut du paiement
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    /// <summary>
    /// Numéro de téléphone utilisé pour le paiement mobile
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Référence de transaction du prestataire de paiement (Wave, Orange Money, etc.)
    /// </summary>
    public string? ProviderTransactionId { get; set; }
    
    /// <summary>
    /// Détails ou métadonnées supplémentaires du paiement
    /// </summary>
    public string? Metadata { get; set; }
    
    /// <summary>
    /// Date et heure de réussite du paiement
    /// </summary>
    public DateTime? SucceededAt { get; set; }
    
    /// <summary>
    /// Date et heure d'échec du paiement
    /// </summary>
    public DateTime? FailedAt { get; set; }
    
    /// <summary>
    /// Raison de l'échec du paiement
    /// </summary>
    public string? FailureReason { get; set; }
    
    /// <summary>
    /// Date et heure du remboursement
    /// </summary>
    public DateTime? RefundedAt { get; set; }
    
    // Foreign Keys
    
    /// <summary>
    /// Identifiant de l'utilisateur qui a effectué le paiement
    /// </summary>
    public Guid UserId { get; set; }
    
    // Navigation properties
    
    /// <summary>
    /// Utilisateur qui a effectué le paiement
    /// </summary>
    public User? User { get; set; }
    
    /// <summary>
    /// Billets associés à ce paiement
    /// </summary>
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
