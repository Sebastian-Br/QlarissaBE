using Qlarissa.Domain.Entities.Securities.Base;
using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Domain.Entities.Securities;

public sealed class Stock : PubliclyTradedSecurityBase
{
    public string InvestorRelationsURL { get; set; }

    public DividendPayout[] DividendPayouts { get; set; }
}