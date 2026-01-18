namespace GestMatch.MauiApp.Services;

/// <summary>
/// Service d'authentification avec Zitadel
/// </summary>
public interface IAuthService
{
    Task<bool> LoginAsync();
    Task LogoutAsync();
    Task<string?> GetAccessTokenAsync();
    bool IsAuthenticated { get; }
    string? UserEmail { get; }
    string? UserName { get; }
}

/// <summary>
/// Implémentation du service d'authentification
/// </summary>
public class AuthService : IAuthService
{
    private string? _accessToken;
    private string? _userEmail;
    private string? _userName;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken);
    public string? UserEmail => _userEmail;
    public string? UserName => _userName;

    public async Task<bool> LoginAsync()
    {
        // TODO: Implémenter l'authentification OIDC avec Zitadel
        // Pour le MVP, simulation d'une authentification réussie
        await Task.Delay(1000);
        
        _accessToken = "mock_token";
        _userEmail = "user@example.com";
        _userName = "Test User";
        
        return true;
    }

    public async Task LogoutAsync()
    {
        await Task.Delay(100);
        
        _accessToken = null;
        _userEmail = null;
        _userName = null;
    }

    public Task<string?> GetAccessTokenAsync()
    {
        return Task.FromResult(_accessToken);
    }
}
