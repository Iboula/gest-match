using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    /// Configure Swagger/OpenAPI avec support OAuth2 pour Zitadel (compatible .NET 8)
    /// </summary>
    public static IServiceCollection AddSwaggerWithOAuth2(this IServiceCollection services, IConfiguration configuration)
    {
        var zitadelAuthority = configuration["Zitadel:Authority"] ?? "http://localhost:8081";
        var zitadelPublicAuthority = configuration["Zitadel:PublicAuthority"] ?? zitadelAuthority;

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "GestMatch API",
                Version = "v1",
                Description = "API de gestion de matchs sportifs et billetterie numérique",
                Contact = new OpenApiContact
                {
                    Name = "GestMatch Team",
                    Email = "contact@gestmatch.sn"
                }
            });

            // Configuration OAuth2 pour Zitadel
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{zitadelPublicAuthority}/oauth/v2/authorize"),
                        TokenUrl = new Uri($"{zitadelPublicAuthority}/oauth/v2/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID Connect" },
                            { "profile", "User profile" },
                            { "email", "User email" },
                            { "gestmatch.matches.create", "Créer des matchs" },
                            { "gestmatch.matches.read", "Lire les matchs" },
                            { "gestmatch.tickets.purchase", "Acheter des billets" },
                            { "gestmatch.tickets.read", "Lire les billets" },
                            { "gestmatch.tickets.scan", "Scanner les billets" }
                        }
                    }
                },
                Description = "ZITADEL OAuth2 Authorization"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new[] { "openid", "profile", "email" }
                }
            });
        });

        return services;
    }
}
