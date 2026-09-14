using Qlarissa.Domain.Securities.Base;

namespace Qlarissa.Domain.Securities;

public sealed class CryptoCurrency : PubliclyTradedSecurityBase
{
    public double MarketCapitalization { get; set; }
}