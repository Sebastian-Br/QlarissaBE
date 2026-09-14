using Qlarissa.Domain.Securities.MarketData;

namespace Qlarissa.Domain.Securities.Base;

public abstract class PubliclyTradedSecurityBase : SecurityBase
{
    public string ExchangeName { get; set; } = string.Empty;

    public string ExchangeShortName { get; set; } = string.Empty;

    /// <summary>
    /// e.g. MSFT for Microsoft.
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    public IReadOnlyList<DailyPrice> PriceHistory {  get; set; } = [];

    /// <summary>
    /// The current market price.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The UTC time at which the Price property has last been updated. This is unrelated to the other properties.
    /// </summary>
    public DateTime PriceLastUpdatedTime {  get; set; }

    /// <summary>
    /// The Date of the last DailyPrice data point in the PriceHistory collection. This is used to determine if the price history is up to date.
    /// </summary>
    public DateOnly PriceHistoryLastDataPointDate {  get; set; }
}