namespace Qlarissa.Domain.Entities.Securities.MarketData;

public sealed class DividendPayout
{
    public DateOnly PayoutDate { get; set; }

    public decimal PayoutAmount { get; set; }
}