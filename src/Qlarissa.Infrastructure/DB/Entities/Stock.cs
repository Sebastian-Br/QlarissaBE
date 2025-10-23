using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class Stock : PubliclyTradedSecurityBase
{
    public string InvestorRelationsURL { get; set; }

    public static Stock FromDomainEntity(Domain.Entities.Securities.Stock domainEntity)
    {
        Stock stock = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, stock);
        stock.DividendPayouts = domainEntity.DividendPayouts.Select(x => DividendPayout.FromDomainEntity(x, domainEntity)).ToList();
        stock.InvestorRelationsURL = domainEntity.InvestorRelationsURL;
        return stock;
    }

    public Domain.Entities.Securities.Stock ToDomainEntity()
    {
        Domain.Entities.Securities.Stock domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        domainEntity.DividendPayouts = DividendPayouts.Select(DividendPayout.ToDomainEntity).ToArray();
        domainEntity.InvestorRelationsURL = InvestorRelationsURL;
        return domainEntity;
    }
}