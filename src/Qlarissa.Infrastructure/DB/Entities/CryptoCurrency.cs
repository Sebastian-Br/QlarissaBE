using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class CryptoCurrency : PubliclyTradedSecurityBase
{
    public double MarketCapitalization { get; set; }

    public static CryptoCurrency FromDomainEntity(Domain.Entities.Securities.CryptoCurrency domainEntity)
    {
        CryptoCurrency dbEntity = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, dbEntity);
        dbEntity.MarketCapitalization = domainEntity.MarketCapitalization;
        return dbEntity;
    }

    public Domain.Entities.Securities.CryptoCurrency ToDomainEntity()
    {
        Domain.Entities.Securities.CryptoCurrency domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        domainEntity.MarketCapitalization = MarketCapitalization;
        return domainEntity;
    }
}