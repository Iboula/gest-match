using GestMatch.Domain.Enums;

namespace GestMatch.Application.DTOs.Users;

/// <summary>
/// DTO pour la réponse d'un utilisateur
/// </summary>
public record UserResponse(
    Guid Id,
    string ZitadelId,
    string Email,
    string FullName,
    string? PhoneNumber,
    UserRole Role,
    bool IsActive,
    string? City,
    DateTime CreatedAt
);

/// <summary>
/// DTO pour la création/mise à jour du profil utilisateur
/// </summary>
public record UpdateUserProfileRequest(
    string? FullName,
    string? PhoneNumber,
    string? City
);
