using Qlarissa.Domain.Securities.Base;
using Qlarissa.Domain.Securities.MarketData;

namespace Qlarissa.Domain.Securities;

public sealed class ETF : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;

    public IReadOnlyList<DividendPayout> DistributionEvents { get; set; } = [];

    public IEnumerable<Split> Splits { get; set; } = [];

    public double NetExpenseRatio { get; set; }

    public double DividendYield { get; set; }
}