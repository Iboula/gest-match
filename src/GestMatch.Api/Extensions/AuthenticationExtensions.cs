using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GestMatch.Api.Extensions;

/// <summary>
/// Extensions pour la configuration de l'authentification et Swagger
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Configure l'authentification Zitadel avec JWT Bearer
    /// </summary>
    public static IServiceCollection AddZitadelAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Zitadel:Authority"]
            ?? throw new InvalidOperationException("Zitadel:Authority is not configured");
        
        var audience = configuration["Zitadel:Audience"]
            ?? throw new InvalidOperationException("Zitadel:Audience is not configured");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };

                // Configuration pour le développement
                if (configuration.GetValue<bool>("Zitadel:RequireHttpsMetadata", true) == false)
                {
                    options.RequireHttpsMetadata = false;
                }
            });

        return services;
    }

    /// <summary>
    /// Configure Swagger avec support de l'authentification JWT
    /// </summary>
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "GestMatch API",
                Description = "API de gestion de matchs sportifs et billetterie numérique",
                Contact = new OpenApiContact
                {
                    Name = "GestMatch Team",
                    Email = "contact@gestmatch.sn"
                }
            });

            // Configuration de la sécurité JWT dans Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Enter your token in the text input below."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
