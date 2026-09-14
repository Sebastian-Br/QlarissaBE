using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class ETF : PubliclyTradedSecurityBase
{
    public string ISIN { get; set; } = string.Empty;

    public double NetExpenseRatio { get; set; }

    public double DividendYield { get; set; }

    internal static ETF FromDomainEntity(Domain.Securities.ETF domainEntity)
    {
        ETF dbEntity = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, dbEntity);
        dbEntity.ISIN = domainEntity.ISIN;
        dbEntity.NetExpenseRatio = domainEntity.NetExpenseRatio;
        dbEntity.DividendYield = domainEntity.DividendYield;
        dbEntity.DividendPayouts = domainEntity.DistributionEvents.Select(x => DividendPayout.FromDomainEntity(x, domainEntity)).ToList();
        dbEntity.Splits = domainEntity.Splits.Select(x => Split.FromDomainEntity(x, domainEntity)).ToList();
        return dbEntity;
    }

    internal new Domain.Securities.ETF ToDomainEntity()
    {
        Domain.Securities.ETF domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        domainEntity.ISIN = ISIN;
        domainEntity.NetExpenseRatio = NetExpenseRatio;
        domainEntity.DividendYield = DividendYield;
        domainEntity.DistributionEvents = DividendPayouts.Select(DividendPayout.ToDomainEntity).ToList();
        domainEntity.Splits = Splits.Select(Split.ToDomainEntity).ToList();
        return domainEntity;
    }

    internal void UpdateFromDbEntity(ETF incomingDbEntity)
    {
        ISIN = incomingDbEntity.ISIN;
        NetExpenseRatio = incomingDbEntity.NetExpenseRatio;
        DividendYield = incomingDbEntity.DividendYield;

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