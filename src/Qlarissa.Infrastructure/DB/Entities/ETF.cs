using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class ETF : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;

    public double NetExpenseRatio { get; set; }

    public double DividendYield { get; set; }

    public static ETF FromDomainEntity(Domain.Entities.Securities.ETF domainEntity)
    {
        ETF dbEntity = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, dbEntity);
        dbEntity.ISIN = domainEntity.ISIN;
        dbEntity.NetExpenseRatio = domainEntity.NetExpenseRatio;
        dbEntity.DividendYield = domainEntity.DividendYield;
        dbEntity.DividendPayouts = domainEntity.DistributionEvents.Select(x => DividendPayout.FromDomainEntity(x, domainEntity)).ToList();
        dbEntity.Splits = domainEntity.Splits.Select(x => Split.FromDomainEntity(x, domainEntity)).ToList();
        return dbEntity;
    }

    public Domain.Entities.Securities.ETF ToDomainEntity()
    {
        Domain.Entities.Securities.ETF domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        domainEntity.ISIN = ISIN;
        domainEntity.NetExpenseRatio = NetExpenseRatio;
        domainEntity.DividendYield = DividendYield;
        domainEntity.DistributionEvents = DividendPayouts.Select(DividendPayout.ToDomainEntity).ToList();
        domainEntity.Splits = Splits.Select(Split.ToDomainEntity).ToList();
        return domainEntity;
    }
}