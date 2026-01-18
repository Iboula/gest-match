using GestMatch.MauiApp.Models;

namespace GestMatch.MauiApp.Services;

/// <summary>
/// Service de gestion des billets
/// </summary>
public interface ITicketService
{
    Task<TicketModel?> PurchaseTicketAsync(Guid matchId, decimal price);
    Task<List<TicketModel>> GetMyTicketsAsync();
    Task<TicketModel?> GetTicketByIdAsync(Guid ticketId);
}

/// <summary>
/// Implémentation du service de billets
/// </summary>
public class TicketService : ITicketService
{
    private readonly IApiService _apiService;

    public TicketService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<TicketModel?> PurchaseTicketAsync(Guid matchId, decimal price)
    {
        var request = new
        {
            MatchId = matchId,
            TicketType = "Standard",
            PaymentMethod = "Wave",
            PhoneNumber = "771234567" // À récupérer de l'utilisateur
        };
        
        return await _apiService.PostAsync<TicketModel>("/api/tickets/purchase", request);
    }

    public async Task<List<TicketModel>> GetMyTicketsAsync()
    {
        var tickets = await _apiService.GetAsync<List<TicketModel>>("/api/tickets/user/me");
        return tickets ?? new List<TicketModel>();
    }

    public async Task<TicketModel?> GetTicketByIdAsync(Guid ticketId)
    {
        return await _apiService.GetAsync<TicketModel>($"/api/tickets/{ticketId}");
    }
}
