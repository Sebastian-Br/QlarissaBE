namespace Qlarissa.Domain.Entities.Securities.MarketData;

public class LivePrice
{
    public string Symbol { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public double Price { get; set; }
    public double DayHigh { get; set; }
    public double DayLow { get; set; }
}