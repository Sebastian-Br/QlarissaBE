namespace Qlarissa.Domain.Entities.Securities.MarketData;

public sealed class DividendPayout
{
    public int Id { get; set; }

    public DateOnly PayoutDate { get; set; }

    public decimal PayoutAmount { get; set; }
}