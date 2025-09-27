using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Domain.Entities.Securities.Base;

public abstract class PubliclyTradedSecurityBase : SecurityBase
{
    /// <summary>
    /// e.g. MSFT for Microsoft or USD for the US-Dollar
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// The first array corresponds to the Date (after it was converted to a double, e.g. January 1st 2024 -> ~2024.1)
    /// The second array is the price history
    /// </summary>
    public Tuple<double[], double[]> PriceHistoryForRegression { get; set; }

    /// <summary>
    /// For display purposes and to quantify volatility, risk, etc.
    /// </summary>
    public DailyPrice[] PriceHistory {  get; set; }

    /// <summary>
    /// The current market price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The UTC time at which the Price property has last been updated. This is unrelated to the other properties.
    /// </summary>
    public DateTime PriceLastUpdatedTime {  get; set; }

    /// <summary>
    /// The UTC time at which all properties have last been updated
    /// </summary>
    public DateTime LastCompleteUpdateTime {  get; set; }

    public string GetDisplayPrice() => Math.Round(Price, 2).ToString() + " " + Currency.Symbol;
}