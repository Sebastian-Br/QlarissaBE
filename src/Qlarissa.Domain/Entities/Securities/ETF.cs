using Qlarissa.Domain.Entities.Securities.Base;
using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Domain.Entities.Securities;

public sealed class ETF : PubliclyTradedSecurityBase
{
    public DividendPayout[] DistributionEvents { get; set; }
}