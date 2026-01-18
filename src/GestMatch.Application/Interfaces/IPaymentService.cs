using GestMatch.Application.DTOs.Payments;
using GestMatch.Domain.Enums;

namespace GestMatch.Application.Interfaces;

/// <summary>
/// Service de gestion des paiements
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Initier un paiement
    /// </summary>
    Task<PaymentResponse> InitiatePaymentAsync(InitiatePaymentRequest request, Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Confirmer un paiement (appelé par webhook du prestataire)
    /// </summary>
    Task<PaymentResponse> ConfirmPaymentAsync(ConfirmPaymentRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir un paiement par référence
    /// </summary>
    Task<PaymentResponse?> GetPaymentByReferenceAsync(string paymentReference, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir les paiements d'un utilisateur
    /// </summary>
    Task<IEnumerable<PaymentResponse>> GetUserPaymentsAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Rembourser un paiement
    /// </summary>
    Task RefundPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default);
}
