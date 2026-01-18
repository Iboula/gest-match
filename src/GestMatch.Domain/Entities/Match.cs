using GestMatch.Domain.Common;
using GestMatch.Domain.Enums;

namespace GestMatch.Domain.Entities;

/// <summary>
/// Entité représentant un match sportif
/// </summary>
public class Match : BaseEntity
{
    /// <summary>
    /// Nom de l'équipe A
    /// </summary>
    public required string TeamA { get; set; }
    
    /// <summary>
    /// Nom de l'équipe B
    /// </summary>
    public required string TeamB { get; set; }
    
    /// <summary>
    /// Date et heure du match
    /// </summary>
    public DateTime MatchDateTime { get; set; }
    
    /// <summary>
    /// Nom du stade ou terrain
    /// </summary>
    public required string Stadium { get; set; }
    
    /// <summary>
    /// Ville où se déroule le match
    /// </summary>
    public required string City { get; set; }
    
    /// <summary>
    /// Région où se déroule le match
    /// </summary>
    public string? Region { get; set; }
    
    /// <summary>
    /// Nom de la compétition ou du tournoi
    /// </summary>
    public required string Competition { get; set; }
    
    /// <summary>
    /// Type de match
    /// </summary>
    public MatchType MatchType { get; set; } = MatchType.Friendly;
    
    /// <summary>
    /// Statut actuel du match
    /// </summary>
    public MatchStatus Status { get; set; } = MatchStatus.Scheduled;
    
    /// <summary>
    /// Description ou notes supplémentaires
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// URL de l'affiche officielle du match
    /// </summary>
    public string? PosterUrl { get; set; }
    
    /// <summary>
    /// Prix du billet standard
    /// </summary>
    public decimal StandardTicketPrice { get; set; }
    
    /// <summary>
    /// Prix du billet VIP
    /// </summary>
    public decimal? VipTicketPrice { get; set; }
    
    /// <summary>
    /// Nombre total de billets disponibles
    /// </summary>
    public int TotalTicketsAvailable { get; set; }
    
    /// <summary>
    /// Nombre de billets VIP disponibles
    /// </summary>
    public int? VipTicketsAvailable { get; set; }
    
    /// <summary>
    /// Date limite de vente des billets
    /// </summary>
    public DateTime? TicketSaleEndDate { get; set; }
    
    /// <summary>
    /// Score de l'équipe A (après le match)
    /// </summary>
    public int? ScoreTeamA { get; set; }
    
    /// <summary>
    /// Score de l'équipe B (après le match)
    /// </summary>
    public int? ScoreTeamB { get; set; }
    
    // Foreign Keys
    
    /// <summary>
    /// Identifiant du gestionnaire qui a créé ce match
    /// </summary>
    public Guid CreatedByUserId { get; set; }
    
    // Navigation properties
    
    /// <summary>
    /// Gestionnaire qui a créé ce match
    /// </summary>
    public User? CreatedByUser { get; set; }
    
    /// <summary>
    /// Billets vendus pour ce match
    /// </summary>
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    /// <summary>
    /// Nombre de billets vendus (propriété calculée)
    /// </summary>
    public int TicketsSold => Tickets.Count(t => t.Status == TicketStatus.Valid || t.Status == TicketStatus.Used);
    
    /// <summary>
    /// Nombre de billets disponibles restants (propriété calculée)
    /// </summary>
    public int TicketsRemaining => TotalTicketsAvailable - TicketsSold;
}
