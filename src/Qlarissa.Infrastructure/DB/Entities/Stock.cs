using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class Stock : PubliclyTradedSecurityBase
{
    public string InvestorRelationsURL { get; set; }
}