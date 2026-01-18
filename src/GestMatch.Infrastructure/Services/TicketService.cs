using GestMatch.Application.DTOs.Tickets;
using GestMatch.Application.Interfaces;
using GestMatch.Domain.Entities;
using GestMatch.Domain.Enums;
using GestMatch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestMatch.Infrastructure.Services;

/// <summary>
/// Implémentation du service de gestion des billets
/// </summary>
public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;
    private readonly IQrCodeService _qrCodeService;

    public TicketService(ApplicationDbContext context, IQrCodeService qrCodeService)
    {
        _context = context;
        _qrCodeService = qrCodeService;
    }

    public async Task<TicketResponse> PurchaseTicketAsync(PurchaseTicketRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        // Vérifier que le match existe et est disponible
        var match = await _context.Matches
            .Include(m => m.Tickets)
            .FirstOrDefaultAsync(m => m.Id == request.MatchId, cancellationToken)
            ?? throw new InvalidOperationException($"Match {request.MatchId} not found");

        // Vérifications
        if (match.Status == MatchStatus.Cancelled)
            throw new InvalidOperationException("Cannot purchase tickets for a cancelled match");

        if (match.TicketSaleEndDate.HasValue && DateTime.UtcNow > match.TicketSaleEndDate.Value)
            throw new InvalidOperationException("Ticket sales have ended for this match");

        var ticketsSold = match.Tickets.Count(t => t.Status == TicketStatus.Valid || t.Status == TicketStatus.Used);
        if (ticketsSold >= match.TotalTicketsAvailable)
            throw new InvalidOperationException("No tickets available for this match");

        // Déterminer le prix
        var price = request.TicketType == TicketType.VIP
            ? match.VipTicketPrice ?? throw new InvalidOperationException("VIP tickets are not available for this match")
            : request.TicketType == TicketType.Free
                ? 0m
                : match.StandardTicketPrice;

        // Créer le paiement si nécessaire
        Payment? payment = null;
        if (price > 0)
        {
            payment = new Payment
            {
                PaymentReference = GeneratePaymentReference(),
                Amount = price,
                PaymentMethod = request.PaymentMethod,
                PhoneNumber = request.PhoneNumber,
                UserId = userId,
                Status = PaymentStatus.Pending
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);

            // TODO: Intégration avec Wave/Orange Money/Free Money
            // Pour le MVP, on considère le paiement comme réussi immédiatement
            payment.Status = PaymentStatus.Succeeded;
            payment.SucceededAt = DateTime.UtcNow;
            payment.ProviderTransactionId = $"MOCK-{Guid.NewGuid().ToString()[..8]}";
        }

        // Créer le billet
        var ticketNumber = GenerateTicketNumber();
        var qrCodeData = _qrCodeService.GenerateQrCodeData(Guid.NewGuid(), ticketNumber);

        var ticket = new Ticket
        {
            TicketNumber = ticketNumber,
            TicketType = request.TicketType,
            Status = TicketStatus.Valid,
            Price = price,
            QrCodeData = qrCodeData,
            HolderName = request.HolderName,
            HolderPhone = request.HolderPhone,
            MatchId = request.MatchId,
            UserId = userId,
            PaymentId = payment?.Id
        };

        // Générer l'image QR Code
        ticket.QrCodeImageUrl = $"data:image/png;base64,{_qrCodeService.GenerateQrCodeImage(qrCodeData)}";

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetTicketResponseAsync(ticket.Id, cancellationToken);
    }

    public async Task<TicketResponse?> GetTicketByIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        return await GetTicketResponseAsync(ticketId, cancellationToken);
    }

    public async Task<IEnumerable<TicketResponse>> GetUserTicketsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tickets = await _context.Tickets
            .Include(t => t.Match)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

        return tickets.Select(MapToTicketResponse);
    }

    public async Task<IEnumerable<TicketResponse>> GetMatchTicketsAsync(Guid matchId, CancellationToken cancellationToken = default)
    {
        var tickets = await _context.Tickets
            .Include(t => t.Match)
            .Where(t => t.MatchId == matchId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

        return tickets.Select(MapToTicketResponse);
    }

    public async Task<ScanTicketResponse> ScanTicketAsync(ScanTicketRequest request, CancellationToken cancellationToken = default)
    {
        // Valider le QR Code
        if (!_qrCodeService.ValidateQrCodeData(request.QrCodeData, out var ticketId))
        {
            return new ScanTicketResponse(false, "QR Code invalide", null);
        }

        // Récupérer le billet
        var ticket = await _context.Tickets
            .Include(t => t.Match)
            .FirstOrDefaultAsync(t => t.Id == ticketId && t.QrCodeData == request.QrCodeData, cancellationToken);

        if (ticket == null)
        {
            return new ScanTicketResponse(false, "Billet introuvable", null);
        }

        // Vérifier le statut
        if (ticket.Status == TicketStatus.Used)
        {
            return new ScanTicketResponse(false, $"Billet déjà utilisé le {ticket.UsedAt:dd/MM/yyyy HH:mm}", MapToTicketResponse(ticket));
        }

        if (ticket.Status == TicketStatus.Cancelled)
        {
            return new ScanTicketResponse(false, "Billet annulé", MapToTicketResponse(ticket));
        }

        if (ticket.Status == TicketStatus.Expired)
        {
            return new ScanTicketResponse(false, "Billet expiré", MapToTicketResponse(ticket));
        }

        // Marquer comme utilisé
        ticket.Status = TicketStatus.Used;
        ticket.UsedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return new ScanTicketResponse(true, "Billet valide - Entrée autorisée", MapToTicketResponse(ticket));
    }

    public async Task CancelTicketAsync(Guid ticketId, Guid userId, CancellationToken cancellationToken = default)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken)
            ?? throw new InvalidOperationException($"Ticket {ticketId} not found");

        if (ticket.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to cancel this ticket");
        }

        if (ticket.Status == TicketStatus.Used)
        {
            throw new InvalidOperationException("Cannot cancel a used ticket");
        }

        ticket.Status = TicketStatus.Cancelled;
        ticket.CancelledAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        // TODO: Initier le remboursement si applicable
    }

    // Méthodes helper privées
    private async Task<TicketResponse> GetTicketResponseAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Match)
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken)
            ?? throw new InvalidOperationException($"Ticket {ticketId} not found");

        return MapToTicketResponse(ticket);
    }

    private static TicketResponse MapToTicketResponse(Ticket ticket)
    {
        var match = ticket.Match!;
        var matchDescription = $"{match.TeamA} vs {match.TeamB} - {match.MatchDateTime:dd/MM/yyyy HH:mm}";

        return new TicketResponse(
            ticket.Id,
            ticket.TicketNumber,
            ticket.TicketType,
            ticket.Status,
            ticket.Price,
            ticket.QrCodeData,
            ticket.QrCodeImageUrl,
            ticket.UsedAt,
            ticket.CancelledAt,
            ticket.HolderName,
            ticket.HolderPhone,
            ticket.MatchId,
            matchDescription,
            match.MatchDateTime,
            match.Stadium,
            match.City,
            ticket.CreatedAt
        );
    }

    private static string GenerateTicketNumber()
    {
        // Format: GM-YYYYMMDD-XXXXX
        return $"GM-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..5].ToUpper()}";
    }

    private static string GeneratePaymentReference()
    {
        // Format: PAY-YYYYMMDD-XXXXX
        return $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
