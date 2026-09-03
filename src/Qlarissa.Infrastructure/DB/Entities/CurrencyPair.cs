
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities;

public class CurrencyPair : PubliclyTradedSecurityBase
{
    public static CurrencyPair FromDomainEntity(Domain.Entities.Securities.CurrencyPair domainEntity)
    {
        CurrencyPair dbEntity = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, dbEntity);
        return dbEntity;
    }

    public Domain.Entities.Securities.CurrencyPair ToDomainEntity()
    {
        Domain.Entities.Securities.CurrencyPair domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        return domainEntity;
    }
}