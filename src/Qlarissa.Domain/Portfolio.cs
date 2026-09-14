namespace Qlarissa.Domain;

public class Portfolio
{

    /// <summary>
    /// The currency of the associated bank account.
    /// </summary>
    public required Currency AccountCurrency { get; set; }
}