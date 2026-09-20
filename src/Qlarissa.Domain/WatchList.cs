namespace Qlarissa.Domain;

public sealed class WatchList
{
    public IEnumerable<WatchedSecurity> WatchedSecurities { get; set; } = [];

    public bool IsPrimary { get; set; }
}