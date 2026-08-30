using Qlarissa.Domain.Entities.Securities.Base;
using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Domain.Entities.Securities;

public sealed class ETF : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;

    public IReadOnlyList<DividendPayout> DistributionEvents { get; set; } = [];

    public IEnumerable<Split> Splits { get; set; } = [];

    public double NetExpenseRatio { get; set; }

    public double DividendYield { get; set; }
}