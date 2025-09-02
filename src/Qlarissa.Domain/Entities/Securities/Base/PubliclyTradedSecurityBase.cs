using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Domain.Entities.Securities.Base;

public abstract class PubliclyTradedSecurityBase : SecurityBase
{
    /// <summary>
    /// e.g. MSFT for Microsoft
    /// </summary>
    public required string Symbol { get; set; }

    /// <summary>
    /// The first array corresponds to the Date (after it was converted to a double, e.g. January 1st 2024 -> ~2024.1)
    /// The second array is the price history
    /// </summary>
    public required Tuple<double[], double[]> PriceHistoryForRegression { get; set; }

    /// <summary>
    /// For display purposes and to quantify volatility, risk, etc.
    /// </summary>
    public required DailyPrice[] PriceHistory {  get; set; }

    /// <summary>
    /// The current market price
    /// </summary>
    public required decimal Price { get; set; }

    public string GetDisplayPrice() => Math.Round(Price, 2).ToString() + " " + Currency.Symbol;
}