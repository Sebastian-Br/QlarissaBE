using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Application.Interfaces.ExternalAPI;

public interface ILivePriceService
{
    /// <summary>
    /// Subscribes a consumer to the specified symbols.
    /// </summary>
    Task SubscribeAsync(string consumerId, IEnumerable<string> symbols, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a consumer's subscriptions for the specified symbols.
    /// </summary>
    Task UnsubscribeAsync(string consumerId, IEnumerable<string> symbols, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes all subscriptions belonging to a consumer.
    /// Should be called when a SignalR connection disconnects.
    /// </summary>
    Task RemoveConsumerAsync(string consumerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the latest known prices for the requested symbols.
    /// Symbols for which no price has been received yet are omitted.
    /// </summary>
    IReadOnlyList<LivePrice> GetPrices(IEnumerable<string> symbols);

    /// <summary>
    /// Prevents the specified symbols from being automatically
    /// unsubscribed while the returned lock is held.
    /// </summary>
    Task<SubscriptionLock> AcquireLockAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default);

    IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetConsumerSubscriptions();

}