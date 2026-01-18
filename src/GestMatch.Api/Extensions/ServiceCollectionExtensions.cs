using GestMatch.Application.Interfaces;
using GestMatch.Infrastructure.Services;

namespace GestMatch.Api.Extensions;

/// <summary>
/// Extensions pour l'enregistrement des services dans le conteneur DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Enregistre tous les services applicatifs
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMatchService, MatchService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IQrCodeService, QrCodeService>();

        return services;
    }
}
