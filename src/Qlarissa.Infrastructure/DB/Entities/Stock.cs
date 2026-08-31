using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class Stock : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;
    public string InvestorRelationsURL { get; set; } = string.Empty;

    public string BusinessSummary { get; set; } = string.Empty;

    public long SharesOutstanding { get; set; }

    public double DividendRate { get; set; }

    public double TargetMeanPrice { get; set; }

    public double RecommendationMean { get; set; }

    public static Stock FromDomainEntity(Domain.Entities.Securities.Stock domainEntity)
    {
        Stock stock = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, stock);
        stock.DividendPayouts = domainEntity.DividendPayouts.Select(x => DividendPayout.FromDomainEntity(x, domainEntity)).ToList();
        stock.Splits = domainEntity.Splits.Select(x => Split.FromDomainEntity(x, domainEntity)).ToList();
        stock.ISIN = domainEntity.ISIN;
        stock.InvestorRelationsURL = domainEntity.InvestorRelationsURL;
        stock.BusinessSummary = domainEntity.BusinessSummary;
        stock.SharesOutstanding = domainEntity.SharesOutstanding;
        stock.DividendRate = domainEntity.DividendRate;
        stock.TargetMeanPrice = domainEntity.TargetMeanPrice;
        stock.RecommendationMean = domainEntity.RecommendationMean;
        return stock;
    }

    public Domain.Entities.Securities.Stock ToDomainEntity()
    {
        Domain.Entities.Securities.Stock domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        domainEntity.DividendPayouts = DividendPayouts.Select(DividendPayout.ToDomainEntity).ToList();
        domainEntity.Splits = Splits.Select(Split.ToDomainEntity).ToList();
        domainEntity.ISIN = ISIN;
        domainEntity.InvestorRelationsURL = InvestorRelationsURL;
        domainEntity.BusinessSummary = BusinessSummary;
        domainEntity.SharesOutstanding = SharesOutstanding;
        domainEntity.DividendRate = DividendRate;
        domainEntity.TargetMeanPrice = TargetMeanPrice;
        domainEntity.RecommendationMean = RecommendationMean;
        return domainEntity;
    }
}