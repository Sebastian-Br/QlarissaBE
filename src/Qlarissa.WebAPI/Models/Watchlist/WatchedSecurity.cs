namespace Qlarissa.WebAPI.Models.Watchlist;

public sealed class WatchedSecurity
{
    public int SecurityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public double PreviousDaysClosePrice { get; set; }

    public static WatchedSecurity FromDomainEntity(Domain.WatchedSecurity domainEntity) => new()
    {
        SecurityId = domainEntity.SecurityId,
        Name = domainEntity.Name,
        Symbol = domainEntity.Symbol,
        PreviousDaysClosePrice = domainEntity.PreviousDaysClosePrice
    };
}