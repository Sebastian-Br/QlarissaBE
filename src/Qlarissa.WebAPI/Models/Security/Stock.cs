using Qlarissa.WebAPI.Models.Security.Base;
using Qlarissa.WebAPI.Models.Security.MarketData;

namespace Qlarissa.WebAPI.Models.Security;

public sealed class Stock : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;

    public string InvestorRelationsURL { get; set; } = string.Empty;

    public string BusinessSummary { get; set; } = string.Empty;

    public long SharesOutstanding { get; set; }

    public double DividendRate { get; set; }

    public double TargetMeanPrice { get; set; }

    public double RecommendationMean { get; set; }

    public IReadOnlyList<DividendPayout> DividendPayouts { get; set; } = [];

    public IEnumerable<Split> Splits { get; set; } = [];

    public static Stock FromDomainEntity(Domain.Entities.Securities.Stock domainEntity)
    {
        Stock stock = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, stock);
        stock.DividendPayouts = domainEntity.DividendPayouts.Select(DividendPayout.FromDomainEntity).ToList();
        stock.Splits = domainEntity.Splits.Select(Split.FromDomainEntity).ToList();
        stock.ISIN = domainEntity.ISIN;
        stock.InvestorRelationsURL = domainEntity.InvestorRelationsURL;
        stock.BusinessSummary = domainEntity.BusinessSummary;
        stock.SharesOutstanding = domainEntity.SharesOutstanding;
        stock.DividendRate = domainEntity.DividendRate;
        stock.TargetMeanPrice = domainEntity.TargetMeanPrice;
        stock.RecommendationMean = domainEntity.RecommendationMean;
        return stock;
    }
}