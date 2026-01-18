using GestMatch.Domain.Entities;
using GestMatch.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GestMatch.Infrastructure.Data.Seed;

/// <summary>
/// Service de seed de données pour la base de données
/// Génère des données de test pour le développement
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seed les données initiales dans la base de données
    /// </summary>
    /// <param name="context">Contexte de base de données</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        // Vérifier si des données existent déjà
        if (await context.Users.AnyAsync(cancellationToken))
        {
            return; // Les données existent déjà
        }

        await SeedUsersAsync(context, cancellationToken);
        await SeedMatchesAsync(context, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed les utilisateurs de test
    /// </summary>
    private static async Task SeedUsersAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var users = new List<User>
        {
            // Admin
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ZitadelId = "admin-zitadel-id",
                Email = "admin@gestmatch.sn",
                FullName = "Administrateur GestMatch",
                PhoneNumber = "+221771234567",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow.AddMonths(-6)
            },
            
            // Gestionnaire de matchs
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ZitadelId = "manager-zitadel-id",
                Email = "manager@gestmatch.sn",
                FullName = "Mamadou Diallo",
                PhoneNumber = "+221772345678",
                Role = UserRole.MatchManager,
                CreatedAt = DateTime.UtcNow.AddMonths(-3)
            },
            
            // Utilisateurs réguliers
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ZitadelId = "user1-zitadel-id",
                Email = "aminata.fall@email.sn",
                FullName = "Aminata Fall",
                PhoneNumber = "+221773456789",
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow.AddMonths(-2)
            },
            
            new()
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                ZitadelId = "user2-zitadel-id",
                Email = "ibrahima.ndiaye@email.sn",
                FullName = "Ibrahima Ndiaye",
                PhoneNumber = "+221774567890",
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow.AddMonths(-1)
            },
            
            new()
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ZitadelId = "user3-zitadel-id",
                Email = "fatou.sarr@email.sn",
                FullName = "Fatou Sarr",
                PhoneNumber = "+221775678901",
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            }
        };

        await context.Users.AddRangeAsync(users, cancellationToken);
    }

    /// <summary>
    /// Seed les matchs de test avec billets
    /// </summary>
    private static async Task SeedMatchesAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var managerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var now = DateTime.UtcNow;

        var matches = new List<Match>
        {
            // Match à venir - Finale CAN
            new()
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                TeamA = "Sénégal",
                TeamB = "Côte d'Ivoire",
                MatchDateTime = now.AddDays(30),
                Stadium = "Stade Abdoulaye Wade",
                City = "Diamniadio",
                Region = "Dakar",
                Competition = "Finale CAN 2026",
                MatchType = GestMatch.Domain.Enums.MatchType.Tournament,
                Status = MatchStatus.Scheduled,
                Description = "Finale tant attendue de la Coupe d'Afrique des Nations 2026",
                PosterUrl = "https://example.com/posters/can-finale-2026.jpg",
                StandardTicketPrice = 25000m,
                VipTicketPrice = 75000m,
                TotalTicketsAvailable = 40000,
                VipTicketsAvailable = 5000,
                TicketSaleEndDate = now.AddDays(29),
                CreatedByUserId = managerId,
                CreatedAt = now.AddDays(-10)
            },

            // Match imminent - Ligue 1
            new()
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                TeamA = "Génération Foot",
                TeamB = "Teungueth FC",
                MatchDateTime = now.AddDays(3),
                Stadium = "Stade Caroline Faye",
                City = "Mbour",
                Region = "Thiès",
                Competition = "Ligue 1 Sénégal - Saison 2025/2026",
                MatchType = GestMatch.Domain.Enums.MatchType.Championship,
                Status = MatchStatus.Scheduled,
                Description = "Choc au sommet de la Ligue 1 sénégalaise",
                StandardTicketPrice = 5000m,
                VipTicketPrice = 15000m,
                TotalTicketsAvailable = 15000,
                VipTicketsAvailable = 1000,
                TicketSaleEndDate = now.AddDays(2).AddHours(12),
                CreatedByUserId = managerId,
                CreatedAt = now.AddDays(-7)
            },

            // Match en cours
            new()
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                TeamA = "Casa Sports",
                TeamB = "Diambars FC",
                MatchDateTime = now.AddHours(-1),
                Stadium = "Stade Aline Sitoé Diatta",
                City = "Ziguinchor",
                Region = "Casamance",
                Competition = "Coupe du Sénégal - Quart de finale",
                MatchType = GestMatch.Domain.Enums.MatchType.Cup,
                Status = MatchStatus.InProgress,
                Description = "Quart de finale de la Coupe du Sénégal",
                StandardTicketPrice = 3000m,
                VipTicketPrice = 10000m,
                TotalTicketsAvailable = 10000,
                VipTicketsAvailable = 500,
                TicketSaleEndDate = now.AddHours(-2),
                ScoreTeamA = 1,
                ScoreTeamB = 1,
                CreatedByUserId = managerId,
                CreatedAt = now.AddDays(-14)
            },

            // Match terminé
            new()
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                TeamA = "Sénégal",
                TeamB = "Mali",
                MatchDateTime = now.AddDays(-5),
                Stadium = "Stade Lat Dior",
                City = "Thiès",
                Region = "Thiès",
                Competition = "Match Amical International",
                MatchType = GestMatch.Domain.Enums.MatchType.Friendly,
                Status = MatchStatus.Finished,
                Description = "Match amical de préparation",
                StandardTicketPrice = 8000m,
                VipTicketPrice = 20000m,
                TotalTicketsAvailable = 20000,
                VipTicketsAvailable = 2000,
                TicketSaleEndDate = now.AddDays(-5).AddHours(-2),
                ScoreTeamA = 2,
                ScoreTeamB = 1,
                CreatedByUserId = managerId,
                CreatedAt = now.AddDays(-20)
            },

            // Match amical à venir
            new()
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                TeamA = "ASC Diaraf",
                TeamB = "US Gorée",
                MatchDateTime = now.AddDays(7),
                Stadium = "Stade Demba Diop",
                City = "Dakar",
                Region = "Dakar",
                Competition = "Match de Gala - Légende du Football",
                MatchType = GestMatch.Domain.Enums.MatchType.Friendly,
                Status = MatchStatus.Scheduled,
                Description = "Match caritatif réunissant les légendes du football sénégalais",
                StandardTicketPrice = 2000m,
                VipTicketPrice = 8000m,
                TotalTicketsAvailable = 25000,
                VipTicketsAvailable = 3000,
                TicketSaleEndDate = now.AddDays(6),
                CreatedByUserId = managerId,
                CreatedAt = now.AddDays(-3)
            },

            // Match annulé
            new()
            {
                Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                TeamA = "Jaraaf",
                TeamB = "Pikine",
                MatchDateTime = now.AddDays(15),
                Stadium = "Stade Demba Diop",
                City = "Dakar",
                Region = "Dakar",
                Competition = "Ligue 1 Sénégal - Saison 2025/2026",
                MatchType = GestMatch.Domain.Enums.MatchType.Championship,
                Status = MatchStatus.Cancelled,
                Description = "Match reporté pour raisons techniques",
                StandardTicketPrice = 4000m,
                VipTicketPrice = 12000m,
                TotalTicketsAvailable = 18000,
                VipTicketsAvailable = 1500,
                TicketSaleEndDate = now.AddDays(14),
                CreatedByUserId = managerId,
                CreatedAt = now.AddDays(-5)
            }
        };

        await context.Matches.AddRangeAsync(matches, cancellationToken);

        // Ajouter des billets vendus pour certains matchs
        await SeedTicketsAsync(context, matches, cancellationToken);
    }

    /// <summary>
    /// Seed les billets de test
    /// </summary>
    private static async Task SeedTicketsAsync(ApplicationDbContext context, List<Match> matches, CancellationToken cancellationToken)
    {
        var user1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var user2Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var user3Id = Guid.Parse("55555555-5555-5555-5555-555555555555");

        var canFinaleMatch = matches.First(m => m.TeamA == "Sénégal" && m.TeamB == "Côte d'Ivoire");
        var ligue1Match = matches.First(m => m.TeamA == "Génération Foot");
        var completedMatch = matches.First(m => m.Status == MatchStatus.Finished);

        var tickets = new List<Ticket>
        {
            // Billets pour la finale CAN
            new()
            {
                Id = Guid.NewGuid(),
                MatchId = canFinaleMatch.Id,
                UserId = user1Id,
                TicketNumber = "GM-20260218-00001",
                TicketType = TicketType.Standard,
                Price = canFinaleMatch.StandardTicketPrice,
                Status = TicketStatus.Valid,
                QrCodeData = "sample-qr-data-001",
                QrCodeImageUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUg==",
                HolderName = "Aminata Fall",
                HolderPhone = "+221773456789",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },

            new()
            {
                Id = Guid.NewGuid(),
                MatchId = canFinaleMatch.Id,
                UserId = user2Id,
                TicketNumber = "GM-20260218-00002",
                TicketType = TicketType.VIP,
                Price = canFinaleMatch.VipTicketPrice ?? 0,
                Status = TicketStatus.Valid,
                QrCodeData = "sample-qr-data-002",
                QrCodeImageUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUg==",
                HolderName = "Ibrahima Ndiaye",
                HolderPhone = "+221774567890",
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            // Billets pour le match de Ligue 1
            new()
            {
                Id = Guid.NewGuid(),
                MatchId = ligue1Match.Id,
                UserId = user3Id,
                TicketNumber = "GM-20260121-00003",
                TicketType = TicketType.Standard,
                Price = ligue1Match.StandardTicketPrice,
                Status = TicketStatus.Valid,
                QrCodeData = "sample-qr-data-003",
                QrCodeImageUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUg==",
                HolderName = "Fatou Sarr",
                HolderPhone = "+221775678901",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },

            // Billet utilisé pour le match terminé
            new()
            {
                Id = Guid.NewGuid(),
                MatchId = completedMatch.Id,
                UserId = user1Id,
                TicketNumber = "GM-20260113-00004",
                TicketType = TicketType.Standard,
                Price = completedMatch.StandardTicketPrice,
                Status = TicketStatus.Used,
                QrCodeData = "sample-qr-data-004",
                QrCodeImageUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUg==",
                HolderName = "Aminata Fall",
                HolderPhone = "+221773456789",
                UsedAt = completedMatch.MatchDateTime.AddHours(-1),
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            }
        };

        await context.Tickets.AddRangeAsync(tickets, cancellationToken);

        // Ajouter les paiements correspondants
        await SeedPaymentsAsync(context, tickets, cancellationToken);
    }

    /// <summary>
    /// Seed les paiements de test
    /// </summary>
    private static async Task SeedPaymentsAsync(ApplicationDbContext context, List<Ticket> tickets, CancellationToken cancellationToken)
    {
        var payments = new List<Payment>();

        foreach (var ticket in tickets)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                UserId = ticket.UserId,
                PaymentReference = $"PAY-{DateTime.UtcNow.Ticks}-{ticket.TicketNumber.Split('-').Last()}",
                Amount = ticket.Price,
                PaymentMethod = ticket.TicketType == TicketType.VIP ? PaymentMethod.CreditCard : 
                               (ticket.TicketNumber.EndsWith("1") ? PaymentMethod.Wave : 
                                ticket.TicketNumber.EndsWith("2") ? PaymentMethod.OrangeMoney : 
                                PaymentMethod.FreeMoney),
                Status = PaymentStatus.Succeeded,
                ProviderTransactionId = $"TXN-{Guid.NewGuid():N}",
                SucceededAt = ticket.CreatedAt.AddMinutes(1),
                CreatedAt = ticket.CreatedAt
            };

            payments.Add(payment);
            
            // Associer le paiement au billet
            ticket.PaymentId = payment.Id;
        }

        await context.Payments.AddRangeAsync(payments, cancellationToken);
    }
}
