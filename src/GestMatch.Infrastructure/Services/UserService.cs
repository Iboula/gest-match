using GestMatch.Application.DTOs.Users;
using GestMatch.Application.Interfaces;
using GestMatch.Domain.Entities;
using GestMatch.Domain.Enums;
using GestMatch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestMatch.Infrastructure.Services;

/// <summary>
/// Implémentation du service de gestion des utilisateurs
/// </summary>
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponse> GetOrCreateUserAsync(string zitadelId, string email, string fullName, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.ZitadelId == zitadelId, cancellationToken);

        if (user == null)
        {
            user = new User
            {
                ZitadelId = zitadelId,
                Email = email,
                FullName = fullName,
                Role = UserRole.User, // Rôle par défaut
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return MapToUserResponse(user);
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user != null ? MapToUserResponse(user) : null;
    }

    public async Task<UserResponse?> GetUserByZitadelIdAsync(string zitadelId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.ZitadelId == zitadelId, cancellationToken);

        return user != null ? MapToUserResponse(user) : null;
    }

    public async Task<UserResponse> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new InvalidOperationException($"User {userId} not found");

        if (request.FullName != null)
            user.FullName = request.FullName;

        if (request.PhoneNumber != null)
            user.PhoneNumber = request.PhoneNumber;

        if (request.City != null)
            user.City = request.City;

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToUserResponse(user);
    }

    private static UserResponse MapToUserResponse(User user)
    {
        return new UserResponse(
            user.Id,
            user.ZitadelId,
            user.Email,
            user.FullName,
            user.PhoneNumber,
            user.Role,
            user.IsActive,
            user.City,
            user.CreatedAt
        );
    }
}
