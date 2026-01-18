using GestMatch.Application.DTOs.Tickets;

namespace GestMatch.Application.Interfaces;

/// <summary>
/// Service de gestion des billets
/// </summary>
public interface ITicketService
{
    /// <summary>
    /// Acheter un billet pour un match
    /// </summary>
    Task<TicketResponse> PurchaseTicketAsync(PurchaseTicketRequest request, Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir un billet par ID
    /// </summary>
    Task<TicketResponse?> GetTicketByIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir les billets d'un utilisateur
    /// </summary>
    Task<IEnumerable<TicketResponse>> GetUserTicketsAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir les billets d'un match
    /// </summary>
    Task<IEnumerable<TicketResponse>> GetMatchTicketsAsync(Guid matchId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Scanner un billet (pour l'entrée au stade)
    /// </summary>
    Task<ScanTicketResponse> ScanTicketAsync(ScanTicketRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Annuler un billet
    /// </summary>
    Task CancelTicketAsync(Guid ticketId, Guid userId, CancellationToken cancellationToken = default);
}
