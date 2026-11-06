namespace Qlarissa.Domain.Watchlists;

public sealed class Watchlist
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsPinned { get; set; }

    // The position of the watchlist in the pinned watchlists list.
    public int PinPosition { get; set; }

    /// <summary>
    /// If set to 1: A daily watch list, i.e. retrieves the previous day's close price to be compared to the current price.
    /// If set to 5: Goes back 5 trading days (or: a trading week), retrieves the close price, to be compared to the current price.
    /// </summary>
    public int TrackedChangeDurationTradingDays { get; set; } = 1;

    public IEnumerable<WatchedSecurity> WatchedSecurities { get; set; } = [];
}