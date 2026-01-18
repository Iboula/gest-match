using GestMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestMatch.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité User
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        // Index sur ZitadelId pour les lookups rapides
        builder.HasIndex(u => u.ZitadelId)
            .IsUnique();

        // Index sur Email
        builder.HasIndex(u => u.Email)
            .IsUnique();

        // Propriétés requises
        builder.Property(u => u.ZitadelId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(u => u.City)
            .HasMaxLength(100);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>(); // Stocké comme string en base

        // Relations
        builder.HasMany(u => u.CreatedMatches)
            .WithOne(m => m.CreatedByUser)
            .HasForeignKey(m => m.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Ne pas supprimer les matchs si l'utilisateur est supprimé

        builder.HasMany(u => u.Tickets)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Payments)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
