using Qlarissa.Domain.Entities.Securities.Holdings;

namespace Qlarissa.Domain.Entities;

public class Portfolio
{

    /// <summary>
    /// The currency of the associated bank account.
    /// </summary>
    public required Currency AccountCurrency { get; set; }
}