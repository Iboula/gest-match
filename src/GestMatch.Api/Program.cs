using GestMatch.Api.Extensions;
using GestMatch.Infrastructure.Data;
using GestMatch.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ====================
// CONFIGURATION DES SERVICES
// ====================

// Configuration de la base de données PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration de l'authentification Zitadel (JWT Bearer)
builder.Services.AddZitadelAuthentication(builder.Configuration);

// Configuration de l'autorisation par rôles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireMatchManager", policy => policy.RequireRole("Admin", "MatchManager"));
    options.AddPolicy("RequireUser", policy => policy.RequireRole("Admin", "MatchManager", "User"));
});

// Enregistrement des services applicatifs
builder.Services.AddApplicationServices();

// Configuration CORS pour l'application mobile
builder.Services.AddCors(options =>
{
    options.AddPolicy("MobileApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuration Swagger avec support JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();

var app = builder.Build();

// ====================
// CONFIGURATION DU PIPELINE HTTP
// ====================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MobileApp");

app.UseAuthentication();
app.UseAuthorization();

// ====================
// DÉFINITION DES ENDPOINTS
// ====================

// Endpoints publics
app.MapGet("/", () => new { Message = "GestMatch API v1.0", Status = "Running" })
    .WithName("Root")
    .WithOpenApi();

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi();

// Groupes d'endpoints
app.MapMatchEndpoints();
app.MapTicketEndpoints();
app.MapUserEndpoints();

// Appliquer les migrations et seed les données (en dev uniquement)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Checking database...");
        
        // Vérifier si la base de données existe et si les tables sont créées
        var canConnect = await dbContext.Database.CanConnectAsync();
        if (!canConnect)
        {
            logger.LogInformation("Database does not exist, creating database...");
            await dbContext.Database.EnsureCreatedAsync();
        }
        
        // Vérifier si les tables existent en essayant de lire la table Users
        var tablesExist = false;
        try
        {
            await dbContext.Database.ExecuteSqlRawAsync("SELECT 1 FROM \"Users\" LIMIT 1");
            tablesExist = true;
            logger.LogInformation("Database schema already exists");
        }
        catch
        {
            logger.LogInformation("Database schema does not exist, creating tables...");
        }
        
        // Créer le schéma si nécessaire
        if (!tablesExist)
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "InitialSchema.sql");
            
            if (File.Exists(sqlScriptPath))
            {
                logger.LogInformation("Executing InitialSchema.sql...");
                var sqlScript = await File.ReadAllTextAsync(sqlScriptPath);
                await dbContext.Database.ExecuteSqlRawAsync(sqlScript);
                logger.LogInformation("Database schema created successfully");
            }
            else
            {
                logger.LogWarning("InitialSchema.sql not found at {Path}, attempting EnsureCreated...", sqlScriptPath);
                await dbContext.Database.EnsureCreatedAsync();
            }
        }
        
        logger.LogInformation("Seeding database with test data...");
        
        // Seed les données de test
        await DatabaseSeeder.SeedAsync(dbContext);
        
        logger.LogInformation("Database setup completed successfully!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while setting up the database");
        throw;
    }
}

app.Run();
