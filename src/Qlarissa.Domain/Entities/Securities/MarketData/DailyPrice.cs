namespace Qlarissa.Domain.Entities.Securities.MarketData;

public sealed class DailyPrice
{
    public int Id { get; set; }
    public double Open { get; set; }
    public double Close { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Average { get; set; }
    public DateOnly Date {  get; set; }
}