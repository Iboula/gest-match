using GestMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestMatch.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité Payment
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        // Index unique sur la référence de paiement
        builder.HasIndex(p => p.PaymentReference)
            .IsUnique();

        // Index sur la référence du prestataire
        builder.HasIndex(p => p.ProviderTransactionId);

        // Index sur le statut
        builder.HasIndex(p => p.Status);

        // Index sur l'utilisateur
        builder.HasIndex(p => p.UserId);

        // Propriétés
        builder.Property(p => p.PaymentReference)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ProviderTransactionId)
            .HasMaxLength(255);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(p => p.Metadata)
            .HasMaxLength(2000);

        builder.Property(p => p.FailureReason)
            .HasMaxLength(500);

        // Conversion des enums
        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

        // Montant avec précision
        builder.Property(p => p.Amount)
            .HasPrecision(18, 2);

        // Relations
        builder.HasMany(p => p.Tickets)
            .WithOne(t => t.Payment)
            .HasForeignKey(t => t.PaymentId)
            .OnDelete(DeleteBehavior.SetNull); // Ne pas supprimer les billets, juste mettre PaymentId à null
    }
}
