using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestMatch.MauiApp.Services;

/// <summary>
/// Service de communication avec l'API
/// </summary>
public interface IApiService
{
    Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PostAsync<T>(string endpoint, object data);
    Task<T?> PutAsync<T>(string endpoint, object data);
    Task<bool> DeleteAsync(string endpoint);
}

/// <summary>
/// Implémentation du service API
/// </summary>
public class ApiService : IApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAuthService _authService;

    public ApiService(IHttpClientFactory httpClientFactory, IAuthService authService)
    {
        _httpClientFactory = httpClientFactory;
        _authService = authService;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var client = await GetHttpClientAsync();
        var response = await client.GetAsync(endpoint);
        
        if (!response.IsSuccessStatusCode)
            return default;
        
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        var client = await GetHttpClientAsync();
        var response = await client.PostAsJsonAsync(endpoint, data);
        
        if (!response.IsSuccessStatusCode)
            return default;
        
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        var client = await GetHttpClientAsync();
        var response = await client.PutAsJsonAsync(endpoint, data);
        
        if (!response.IsSuccessStatusCode)
            return default;
        
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        var client = await GetHttpClientAsync();
        var response = await client.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }

    private async Task<HttpClient> GetHttpClientAsync()
    {
        var client = _httpClientFactory.CreateClient("GestMatchApi");
        
        // Ajouter le token d'authentification
        var token = await _authService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return client;
    }
}
