using GestMatch.MauiApp.Models;

namespace GestMatch.MauiApp.Services;

/// <summary>
/// Service de gestion des matchs
/// </summary>
public interface IMatchService
{
    Task<List<MatchModel>> GetMatchesAsync(string? city = null, DateTime? fromDate = null);
    Task<MatchModel?> GetMatchByIdAsync(Guid matchId);
}

/// <summary>
/// Implémentation du service de matchs
/// </summary>
public class MatchService : IMatchService
{
    private readonly IApiService _apiService;

    public MatchService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<MatchModel>> GetMatchesAsync(string? city = null, DateTime? fromDate = null)
    {
        var endpoint = "/api/matches";
        var queryParams = new List<string>();
        
        if (!string.IsNullOrEmpty(city))
            queryParams.Add($"city={Uri.EscapeDataString(city)}");
        
        if (fromDate.HasValue)
            queryParams.Add($"fromDate={fromDate.Value:yyyy-MM-dd}");
        
        if (queryParams.Any())
            endpoint += "?" + string.Join("&", queryParams);
        
        var matches = await _apiService.GetAsync<List<MatchModel>>(endpoint);
        return matches ?? new List<MatchModel>();
    }

    public async Task<MatchModel?> GetMatchByIdAsync(Guid matchId)
    {
        return await _apiService.GetAsync<MatchModel>($"/api/matches/{matchId}");
    }
}
