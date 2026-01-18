using GestMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestMatch.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité Ticket
/// </summary>
public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        // Index unique sur le numéro de billet
        builder.HasIndex(t => t.TicketNumber)
            .IsUnique();

        // Index sur QR Code pour la vérification rapide
        builder.HasIndex(t => t.QrCodeData)
            .IsUnique();

        // Index sur le statut
        builder.HasIndex(t => t.Status);

        // Index sur l'utilisateur
        builder.HasIndex(t => t.UserId);

        // Index sur le match
        builder.HasIndex(t => t.MatchId);

        // Propriétés
        builder.Property(t => t.TicketNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.QrCodeData)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.QrCodeImageUrl)
            .HasMaxLength(500);

        builder.Property(t => t.HolderName)
            .HasMaxLength(255);

        builder.Property(t => t.HolderPhone)
            .HasMaxLength(20);

        // Conversion des enums
        builder.Property(t => t.TicketType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>();

        // Prix avec précision
        builder.Property(t => t.Price)
            .HasPrecision(18, 2);

        // Relations configurées dans UserConfiguration et MatchConfiguration
    }
}
