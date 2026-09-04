using Qlarissa.WebAPI.Models.Security.Base;
using Qlarissa.WebAPI.Models.Security.MarketData;

namespace Qlarissa.WebAPI.Models.Security;

public sealed class ETF : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;

    public IReadOnlyList<DividendPayout> DistributionEvents { get; set; } = [];

    public IEnumerable<Split> Splits { get; set; } = [];

    public double NetExpenseRatio { get; set; }

    public double DividendYield { get; set; }

    public static ETF FromDomainEntity(Domain.Entities.Securities.ETF domainEntity)
    {
        ETF etf = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, etf);
        etf.DistributionEvents = domainEntity.DistributionEvents.Select(DividendPayout.FromDomainEntity).ToList();
        etf.Splits = domainEntity.Splits.Select(Split.FromDomainEntity).ToList();
        etf.ISIN = domainEntity.ISIN;
        etf.NetExpenseRatio = domainEntity.NetExpenseRatio;
        etf.DividendYield = domainEntity.DividendYield;
        return etf;
    }
}