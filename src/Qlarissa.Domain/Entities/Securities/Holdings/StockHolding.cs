namespace Qlarissa.Domain.Entities.Securities.Holdings;

public sealed class StockHolding
{
    public required Stock Stock { get; set; }

    public int Amount { get; set; }
}