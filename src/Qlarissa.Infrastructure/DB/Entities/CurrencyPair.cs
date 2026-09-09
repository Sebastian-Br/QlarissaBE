
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities;

public class CurrencyPair : PubliclyTradedSecurityBase
{
    internal static CurrencyPair FromDomainEntity(Domain.Entities.Securities.CurrencyPair domainEntity)
    {
        CurrencyPair dbEntity = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, dbEntity);
        return dbEntity;
    }

    internal new Domain.Entities.Securities.CurrencyPair ToDomainEntity()
    {
        Domain.Entities.Securities.CurrencyPair domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        return domainEntity;
    }

    internal void UpdateFromDbEntity(CurrencyPair incomingDbEntity)
    {
        if (incomingDbEntity.PriceHistory.First().Date <= PriceHistoryLastDataPointDate)
        {
            // We expect only new data to be present in the incoming entity.
            throw new InvalidOperationException("Incoming history is attempting to update existing entries, but no new splits have occurred.");
        }

        foreach (var price in incomingDbEntity.PriceHistory)
        {
            PriceHistory.Add(price); // We can trust the incoming price history to be in chronological order. It is ordered by the IMarketDataClient.
        }
    }
}