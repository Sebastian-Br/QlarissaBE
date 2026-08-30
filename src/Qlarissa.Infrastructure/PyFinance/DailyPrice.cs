using Qlarissa.Domain.Entities.Securities.MarketData;
using System.Text.Json.Serialization;

namespace Qlarissa.Infrastructure.PyFinance;

public class DailyPrice
{
    public DateOnly Date { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Open { get; set; }
    public double Close { get; set; }
    public double Dividends { get; set; }
    
    [JsonPropertyName("Stock Splits")]
    public double StockSplits { get; set; }

    public Qlarissa.Domain.Entities.Securities.MarketData.DailyPrice ToDomainEntity()
    {
        return new Qlarissa.Domain.Entities.Securities.MarketData.DailyPrice
        {
            Date = Date,
            High = High,
            Low = Low,
            Open = Open,
            Close = Close,
            Average = (Open + Close) / 2.0
        };
    }
}