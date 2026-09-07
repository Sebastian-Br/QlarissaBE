namespace Qlarissa.Infrastructure.PyFinance.Options;

public sealed class LivePriceOptions
{
    /// <summary>
    /// How frequently changed prices are batched for consumers.
    /// Default: 5 seconds.
    /// </summary>
    public TimeSpan BatchInterval { get; set; } =
        TimeSpan.FromSeconds(5);

    /// <summary>
    /// Initial reconnect delay after a Yahoo connection failure.
    /// </summary>
    public TimeSpan InitialReconnectDelay { get; set; } =
        TimeSpan.FromSeconds(1);

    /// <summary>
    /// Maximum reconnect delay after repeated Yahoo failures.
    /// </summary>
    public TimeSpan MaximumReconnectDelay { get; set; } =
        TimeSpan.FromSeconds(30);
}