using GestMatch.Domain.Enums;

namespace GestMatch.Application.DTOs.Tickets;

/// <summary>
/// DTO pour l'achat d'un billet
/// </summary>
public record PurchaseTicketRequest(
    Guid MatchId,
    TicketType TicketType,
    PaymentMethod PaymentMethod,
    string? PhoneNumber,  // Requis pour paiements mobiles
    string? HolderName,
    string? HolderPhone
);

/// <summary>
/// DTO pour la réponse d'un billet
/// </summary>
public record TicketResponse(
    Guid Id,
    string TicketNumber,
    TicketType TicketType,
    TicketStatus Status,
    decimal Price,
    string QrCodeData,
    string? QrCodeImageUrl,
    DateTime? UsedAt,
    DateTime? CancelledAt,
    string? HolderName,
    string? HolderPhone,
    Guid MatchId,
    string MatchDescription,  // "Team A vs Team B - Date"
    DateTime MatchDateTime,
    string Stadium,
    string City,
    DateTime CreatedAt
);

/// <summary>
/// DTO pour scanner un billet
/// </summary>
public record ScanTicketRequest(
    string QrCodeData
);

/// <summary>
/// DTO pour la réponse du scan d'un billet
/// </summary>
public record ScanTicketResponse(
    bool IsValid,
    string Message,
    TicketResponse? Ticket
);
