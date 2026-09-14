using Qlarissa.Domain.Securities.Base;
using Qlarissa.Domain.Securities.MarketData;

namespace Qlarissa.Domain.Securities;

public sealed class Stock : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;

    public string InvestorRelationsURL { get; set; } = string.Empty;

    public string BusinessSummary { get; set; } = string.Empty;

    public long SharesOutstanding { get; set; }

    public double DividendRate { get; set; }

    public double TargetMeanPrice { get; set; }

    public double RecommendationMean { get; set; }

    public IReadOnlyList<DividendPayout> DividendPayouts { get; set; } = [];

    public IEnumerable<Split> Splits { get; set; } = [];
}