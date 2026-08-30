using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Domain.Entities.Securities;

public class CryptoCurrency : PubliclyTradedSecurityBase
{
    public double MarketCapitalization { get; set; }
}