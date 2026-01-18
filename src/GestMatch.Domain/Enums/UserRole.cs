namespace GestMatch.Domain.Enums;

/// <summary>
/// Rôles des utilisateurs dans le système
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Utilisateur simple - peut consulter et acheter des billets
    /// </summary>
    User = 0,
    
    /// <summary>
    /// Gestionnaire de matchs - peut créer/modifier/annuler des matchs
    /// </summary>
    MatchManager = 1,
    
    /// <summary>
    /// Administrateur - accès complet au système
    /// </summary>
    Admin = 2
}
