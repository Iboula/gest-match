namespace GestMatch.Domain.Common;

/// <summary>
/// Classe de base pour toutes les entités
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identifiant unique de l'entité
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Date de création de l'entité
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Date de dernière modification
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
