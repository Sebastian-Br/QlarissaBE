namespace Qlarissa.Domain.Entities.Securities.MarketData;

public sealed class DailyPrice
{
    public decimal Open { get; set; }
    public decimal Close { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Average { get; set; }
    public DateOnly Date {  get; set; }
}