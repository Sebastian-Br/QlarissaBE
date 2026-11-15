namespace Qlarissa.Domain.Portfolios;

public sealed class CurrencyHolding
{
    public required Currency Currency { get; set; }
    public double Amount { get; set; }
}