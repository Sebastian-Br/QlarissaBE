using Qlarissa.WebAPI.Models.Security.Base;

namespace Qlarissa.WebAPI.Models.Security;

/// <summary>
/// In a CurrencyPair, e.g. USD-EUR, USD would be the primary currency.
/// this.Symbol would be USD whereas this.Currency would refer to the Euro
/// </summary>
public sealed class CurrencyPair : PubliclyTradedSecurityBase
{
    public static CurrencyPair FromDomainEntity(Domain.Entities.Securities.CurrencyPair domainEntity)
    {
        CurrencyPair currencyPair = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, currencyPair);
        return currencyPair;
    }
}