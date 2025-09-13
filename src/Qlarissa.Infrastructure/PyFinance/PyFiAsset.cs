namespace Qlarissa.Infrastructure.PyFinance;

public class PyFiAsset
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string ISIN { get; set; }
    public string Currency { get; set; }
    public long MarketCapitalization { get; set; }
    public double TrailingPE { get; set; }
    public double ForwardPE { get; set; }
    public decimal DividendPerShareYearly { get; set; }
    public string InvestorRelationsWebsite { get; set; }
    public decimal TargetMeanPrice { get; set; }
    public double RecommendationMean { get; set; }
    public int NumberOfAnalystOpinions { get; set; }
    public long SharesOutstanding { get; set; }
    public Dictionary<string, PyFiDailyPrice> HistoricalData { get; set; }
    public Dictionary<string, decimal> DividendHistory { get; set; }
}