namespace GestMatch.Domain.Enums;

/// <summary>
/// Méthode de paiement (adaptée au Sénégal)
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Wave (mobile money)
    /// </summary>
    Wave = 0,
    
    /// <summary>
    /// Orange Money
    /// </summary>
    OrangeMoney = 1,
    
    /// <summary>
    /// Free Money
    /// </summary>
    FreeMoney = 2,
    
    /// <summary>
    /// Carte bancaire
    /// </summary>
    CreditCard = 3,
    
    /// <summary>
    /// Gratuit (pour billets gratuits)
    /// </summary>
    Free = 4
}
