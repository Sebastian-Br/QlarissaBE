using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class ETF : PubliclyTradedSecurityBase
{
    public static ETF FromDomainEntity(Domain.Entities.Securities.ETF domainEntity)
    {
        ETF etf = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, etf);
        etf.DividendPayouts = domainEntity.DistributionEvents.Select(x => DividendPayout.FromDomainEntity(x, domainEntity)).ToList();
        return etf;
    }

    public Domain.Entities.Securities.ETF ToDomainEntity()
    {
        Domain.Entities.Securities.ETF domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        return domainEntity;
    }
}