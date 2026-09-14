namespace Qlarissa.Domain.Securities.MarketData;

public sealed class DividendPayout
{
    public int Id { get; set; }

    public DateOnly PayoutDate { get; set; }

    public double PayoutAmount { get; set; }
}