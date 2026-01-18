using GestMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestMatch.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité Match
/// </summary>
public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(m => m.Id);

        // Index sur la date du match pour les requêtes de recherche
        builder.HasIndex(m => m.MatchDateTime);

        // Index sur la ville
        builder.HasIndex(m => m.City);

        // Index sur le statut
        builder.HasIndex(m => m.Status);

        // Index composé pour recherche par ville + date
        builder.HasIndex(m => new { m.City, m.MatchDateTime });

        // Propriétés
        builder.Property(m => m.TeamA)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.TeamB)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.Stadium)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Region)
            .HasMaxLength(100);

        builder.Property(m => m.Competition)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.Description)
            .HasMaxLength(2000);

        builder.Property(m => m.PosterUrl)
            .HasMaxLength(500);

        // Conversion des enums en string
        builder.Property(m => m.MatchType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(m => m.Status)
            .IsRequired()
            .HasConversion<string>();

        // Prix avec précision décimale
        builder.Property(m => m.StandardTicketPrice)
            .HasPrecision(18, 2);

        builder.Property(m => m.VipTicketPrice)
            .HasPrecision(18, 2);

        // Ignorer les propriétés calculées
        builder.Ignore(m => m.TicketsSold);
        builder.Ignore(m => m.TicketsRemaining);

        // Relations
        builder.HasMany(m => m.Tickets)
            .WithOne(t => t.Match)
            .HasForeignKey(t => t.MatchId)
            .OnDelete(DeleteBehavior.Cascade); // Supprimer les billets si le match est supprimé
    }
}
