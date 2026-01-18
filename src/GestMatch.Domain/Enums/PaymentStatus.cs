namespace GestMatch.Domain.Enums;

/// <summary>
/// Statut d'un paiement
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Paiement en attente
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// Paiement réussi
    /// </summary>
    Succeeded = 1,
    
    /// <summary>
    /// Paiement échoué
    /// </summary>
    Failed = 2,
    
    /// <summary>
    /// Paiement remboursé
    /// </summary>
    Refunded = 3
}
