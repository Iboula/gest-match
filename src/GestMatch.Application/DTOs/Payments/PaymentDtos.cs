using GestMatch.Domain.Enums;

namespace GestMatch.Application.DTOs.Payments;

/// <summary>
/// DTO pour la réponse d'un paiement
/// </summary>
public record PaymentResponse(
    Guid Id,
    string PaymentReference,
    decimal Amount,
    PaymentMethod PaymentMethod,
    PaymentStatus Status,
    string? PhoneNumber,
    string? ProviderTransactionId,
    DateTime? SucceededAt,
    DateTime? FailedAt,
    string? FailureReason,
    DateTime CreatedAt
);

/// <summary>
/// DTO pour initier un paiement
/// </summary>
public record InitiatePaymentRequest(
    decimal Amount,
    PaymentMethod PaymentMethod,
    string? PhoneNumber,
    string? Metadata
);

/// <summary>
/// DTO pour confirmer un paiement (webhook des prestataires)
/// </summary>
public record ConfirmPaymentRequest(
    string PaymentReference,
    string ProviderTransactionId,
    PaymentStatus Status,
    string? FailureReason
);
