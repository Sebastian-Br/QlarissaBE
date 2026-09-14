namespace Qlarissa.Domain.WatchList;

public sealed class WatchList
{
    public int Id { get; set; }

    public IEnumerable<WatchedSecurity> WatchedSecurities { get; set; } = [];
}