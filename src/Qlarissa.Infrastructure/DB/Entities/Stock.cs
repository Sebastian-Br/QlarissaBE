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

    internal static Stock FromDomainEntity(Domain.Entities.Securities.Stock domainEntity)
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

    internal new Domain.Entities.Securities.Stock ToDomainEntity()
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

    internal void UpdateFromDbEntity(Stock incomingDbEntity)
    {
        ISIN = incomingDbEntity.ISIN;
        InvestorRelationsURL = incomingDbEntity.InvestorRelationsURL;
        BusinessSummary = incomingDbEntity.BusinessSummary;
        SharesOutstanding = incomingDbEntity.SharesOutstanding;
        DividendRate = incomingDbEntity.DividendRate;
        TargetMeanPrice = incomingDbEntity.TargetMeanPrice;
        RecommendationMean = incomingDbEntity.RecommendationMean;

        if (incomingDbEntity.Splits.Any(split => split.Date > PriceHistoryLastDataPointDate))
        {
            // Update existing price history.
            var existingPriceHistory = PriceHistory.ToDictionary(dailyPrice => dailyPrice.Date);
            foreach (var incomingPrice in incomingDbEntity.PriceHistory)
            {
                if (existingPriceHistory.TryGetValue(incomingPrice.Date, out var existingPrice))
                {
                    existingPrice.UpdateFromDbEntity(incomingPrice);
                }
                else
                {
                    PriceHistory.Add(incomingPrice);
                }
            }

            var newDividendPayouts = incomingDbEntity.DividendPayouts.Where(dividend => dividend.PayoutDate > DividendPayouts.Last().PayoutDate);
            foreach (var newPayout in newDividendPayouts)
            {
                DividendPayouts.Add(newPayout);
            }

            var newSplits = incomingDbEntity.Splits.Where(split => split.Date > Splits.Last().Date);
            foreach (var newSplit in newSplits)
            {
                Splits.Add(newSplit);
            }
        }
        else // No new splits - just add new data instead of updating existing history.
        {
            if (incomingDbEntity.PriceHistory.FirstOrDefault()?.Date <= PriceHistoryLastDataPointDate
                || incomingDbEntity.DividendPayouts.FirstOrDefault()?.PayoutDate <= DividendPayouts.LastOrDefault()?.PayoutDate
                || incomingDbEntity.Splits.FirstOrDefault()?.Date <= Splits.LastOrDefault()?.Date)
            {
                // We expect only new data to be present in the incoming entity.
                throw new InvalidOperationException("Incoming history is attempting to update existing entries, but no new splits have occurred.");
            }

            foreach (var price in incomingDbEntity.PriceHistory)
            {
                PriceHistory.Add(price); // We can trust the incoming price history to be in chronological order. It is ordered by the IMarketDataClient.
            }

            foreach (var dividend in incomingDbEntity.DividendPayouts)
            {
                DividendPayouts.Add(dividend);
            }

            foreach (var split in incomingDbEntity.Splits)
            {
                Splits.Add(split);
            }
        }
    }
}