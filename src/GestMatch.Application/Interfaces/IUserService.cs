using GestMatch.Application.DTOs.Users;

namespace GestMatch.Application.Interfaces;

/// <summary>
/// Service de gestion des utilisateurs
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Obtenir ou créer un utilisateur à partir des informations Zitadel
    /// </summary>
    Task<UserResponse> GetOrCreateUserAsync(string zitadelId, string email, string fullName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir un utilisateur par ID
    /// </summary>
    Task<UserResponse?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtenir un utilisateur par Zitadel ID
    /// </summary>
    Task<UserResponse?> GetUserByZitadelIdAsync(string zitadelId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Mettre à jour le profil utilisateur
    /// </summary>
    Task<UserResponse> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default);
}
