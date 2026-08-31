namespace Qlarissa.Infrastructure.PyFinance;

public class SecurityInformation
{
    /// <summary>
    /// Filled for all quote types.
    /// </summary>
    public string QuoteType { get; init; } = string.Empty;

    /// <summary>
    /// Filled for all quote types.
    /// </summary>
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Filled for all quote types.
    /// </summary>
    public string LongName { get; init; } = string.Empty;

    /// <summary>
    /// Filled for all quote types.
    /// </summary>
    public string ShortName { get; init; } = string.Empty;

    /// <summary>
    /// Filled for ETFs and Stocks.
    /// </summary>
    public string LongBusinessSummary { get; init; } = string.Empty;

    /// <summary>
    /// Filled for all quote types.
    /// </summary>
    public double FullDayPrice { get; init; }

    /// <summary>
    /// Filled for all quote types.
    /// </summary>
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// Filled for all quote types.
    /// </summary>
    public string Exchange { get; init; } = string.Empty;

    /// <summary>
    /// Filled for all quote types.
    /// </summary>
    public string FullExchangeName { get; init; } = string.Empty;

    /// <summary>
    /// Filled for Stocks and Cryptocurrencies.
    /// </summary>
    public long MarketCap { get; init; }

    /// <summary>
    /// Filled for Stocks.
    /// </summary>
    public long SharesOutstanding { get; init; }

    /// <summary>
    /// Filled for Stocks.
    /// </summary>
    public double DividendRate { get; init; }

    /// <summary>
    /// Filled for ETFs.
    /// </summary>
    public double DividendYield { get; init; }

    /// <summary>
    /// Filled for Stocks.
    /// </summary>
    public string IrWebsite { get; init; } = string.Empty;

    /// <summary>
    /// Filled for Stocks.
    /// </summary>
    public double TargetMeanPrice { get; init; }

    /// <summary>
    /// Filled for Stocks.
    /// </summary>
    public double RecommendationMean { get; init; }

    /// <summary>
    /// Filled for ETFs.
    /// </summary>
    public double NetExpenseRatio { get; init; }
}