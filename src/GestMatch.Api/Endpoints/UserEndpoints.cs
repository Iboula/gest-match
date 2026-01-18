using System.Security.Claims;
using GestMatch.Application.DTOs.Users;
using GestMatch.Application.Interfaces;

namespace GestMatch.Api.Extensions;

/// <summary>
/// Extensions pour les endpoints de gestion des utilisateurs
/// </summary>
public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithOpenApi();

        // GET /api/users/me - Mon profil
        group.MapGet("/me", async (
            IUserService userService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var zitadelId = user.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException("Zitadel ID not found");

            var email = user.FindFirst(ClaimTypes.Email)?.Value
                ?? user.FindFirst("email")?.Value
                ?? throw new UnauthorizedAccessException("Email not found");

            var fullName = user.FindFirst(ClaimTypes.Name)?.Value
                ?? user.FindFirst("name")?.Value
                ?? "Unknown User";

            var userResponse = await userService.GetOrCreateUserAsync(zitadelId, email, fullName, cancellationToken);
            return Results.Ok(userResponse);
        })
        .WithName("GetMyProfile")
        .WithSummary("Obtenir mon profil utilisateur")
        .RequireAuthorization()
        .Produces<UserResponse>();

        // PUT /api/users/me - Mettre à jour mon profil
        group.MapPut("/me", async (
            UpdateUserProfileRequest request,
            IUserService userService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var userId = GetUserId(user);
            var updatedUser = await userService.UpdateUserProfileAsync(userId, request, cancellationToken);
            return Results.Ok(updatedUser);
        })
        .WithName("UpdateMyProfile")
        .WithSummary("Mettre à jour mon profil")
        .RequireAuthorization()
        .Produces<UserResponse>();

        // GET /api/users/{id} - Profil d'un utilisateur
        group.MapGet("/{id:guid}", async (
            Guid id,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            var user = await userService.GetUserByIdAsync(id, cancellationToken);
            return user != null ? Results.Ok(user) : Results.NotFound();
        })
        .WithName("GetUserById")
        .WithSummary("Obtenir le profil d'un utilisateur par ID")
        .RequireAuthorization()
        .Produces<UserResponse>()
        .Produces(StatusCodes.Status404NotFound);
    }

    private static Guid GetUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.FindFirst("sub")?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in claims");

        return Guid.TryParse(userIdClaim, out var userId) ? userId : throw new UnauthorizedAccessException("Invalid user ID");
    }
}
