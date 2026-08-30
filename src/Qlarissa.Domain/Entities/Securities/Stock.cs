using Qlarissa.Domain.Entities.Securities.Base;
using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Domain.Entities.Securities;

public sealed class Stock : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;

    public string InvestorRelationsURL { get; set; } = string.Empty;

    public string BusinessSummary { get; set; } = string.Empty;

    public long MarketCapitalization { get; set; }

    public double DividendRate { get; set; }

    public double TargetMeanPrice { get; set; }

    public double RecommendationMean { get; set; }

    public IReadOnlyList<DividendPayout> DividendPayouts { get; set; } = [];

    public IEnumerable<Split> Splits { get; set; } = [];
}