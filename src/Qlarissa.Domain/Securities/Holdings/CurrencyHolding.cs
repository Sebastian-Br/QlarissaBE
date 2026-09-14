namespace Qlarissa.Domain.Securities.Holdings;

public sealed class CurrencyHolding
{
    public required Currency Currency { get; set; }
    public double Amount { get; set; }
}