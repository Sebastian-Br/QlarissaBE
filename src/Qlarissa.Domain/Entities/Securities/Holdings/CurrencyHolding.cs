namespace Qlarissa.Domain.Entities.Securities.Holdings;

public sealed class CurrencyHolding
{
    public required Currency Currency { get; set; }
    public decimal Amount { get; set; }
}