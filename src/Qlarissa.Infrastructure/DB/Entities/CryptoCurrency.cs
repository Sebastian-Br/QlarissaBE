using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class CryptoCurrency : PubliclyTradedSecurityBase
{
    public double MarketCapitalization { get; set; }

    internal static CryptoCurrency FromDomainEntity(Domain.Entities.Securities.CryptoCurrency domainEntity)
    {
        CryptoCurrency dbEntity = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, dbEntity);
        dbEntity.MarketCapitalization = domainEntity.MarketCapitalization;
        return dbEntity;
    }

    internal new Domain.Entities.Securities.CryptoCurrency ToDomainEntity()
    {
        Domain.Entities.Securities.CryptoCurrency domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, this);
        domainEntity.MarketCapitalization = MarketCapitalization;
        return domainEntity;
    }

    internal void UpdateFromDbEntity(CryptoCurrency incomingDbEntity)
    {
        MarketCapitalization = incomingDbEntity.MarketCapitalization;

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