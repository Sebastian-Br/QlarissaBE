using Qlarissa.WebAPI.Models.Security.Base;

namespace Qlarissa.WebAPI.Models.Security;

public sealed class CryptoCurrency : PubliclyTradedSecurityBase
{
    public double MarketCapitalization { get; set; }

    public static CryptoCurrency FromDomainEntity(Domain.Entities.Securities.CryptoCurrency domainEntity)
    {
        CryptoCurrency cryptoCurrency = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, cryptoCurrency);
        cryptoCurrency.MarketCapitalization = domainEntity.MarketCapitalization;
        return cryptoCurrency;
    }
}