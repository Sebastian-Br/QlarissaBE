namespace Qlarissa.Infrastructure.PyFinance;

internal class SecurityInformation
{
    public string QuoteType { get; init; } = string.Empty;
    public string Symbol { get; init; } = string.Empty;
    public string LongName { get; init; } = string.Empty;
    public string LongBusinessSummary { get; init; } = string.Empty;
    public string Currency { get; init; } = string.Empty;
    public string FullExchangeName { get; init; } = string.Empty;
    public long MarketCap { get; init; }
    public double DividendYield { get; init; }
    public string IrWebsite { get; init; } = string.Empty;
    public decimal TargetMeanPrice { get; init; }
    public double RecommendationMean { get; init; }

    // ETF

    public double NetExpenseRatio { get; init; }
}