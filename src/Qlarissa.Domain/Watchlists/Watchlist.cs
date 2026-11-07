namespace Qlarissa.Domain.Watchlists;

public sealed class Watchlist : WatchlistMinimal
{
    public IEnumerable<WatchedSecurity> WatchedSecurities { get; set; } = [];
}