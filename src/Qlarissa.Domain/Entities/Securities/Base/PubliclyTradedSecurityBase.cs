using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Domain.Entities.Securities.Base;

public abstract class PubliclyTradedSecurityBase : SecurityBase
{
    public string ExchangeName { get; set; }

    public string ExchangeShortName { get; set; }

    /// <summary>
    /// e.g. MSFT for Microsoft.
    /// </summary>
    public string Symbol { get; set; }

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
    /// The UTC time at which all properties have last been updated.
    /// </summary>
    public DateTime LastCompleteUpdateTime {  get; set; }

    public string GetDisplayPrice() => Math.Round(Price, 2).ToString() + " " + Currency.Symbol;
}