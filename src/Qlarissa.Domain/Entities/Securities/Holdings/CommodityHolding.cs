namespace Qlarissa.Domain.Entities.Securities.Holdings;

public sealed class CommodityHolding
{
    public required Commodity Commodity { get; set; }

    public int Amount { get; set; }
}