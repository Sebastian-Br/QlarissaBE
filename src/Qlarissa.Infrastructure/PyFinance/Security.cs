namespace Qlarissa.Infrastructure.PyFinance;

public class Security
{
    public string Symbol { get; init; } = string.Empty;
    public string LongName { get; init; } = string.Empty;
    public string Currency { get; init; } = string.Empty;
    public long MarketCap { get; init ; }
    public double TrailingPE { get; set; }
    public double ForwardPE { get; set; }
    public decimal DividendPerShareYearly { get; set; }
    public string InvestorRelationsWebsite { get; set; }
    public decimal TargetMeanPrice { get; set; }
    public double RecommendationMean { get; set; }
    public int NumberOfAnalystOpinions { get; set; }
    public IEnumerable<DailyPrice> History { get; set; }
}