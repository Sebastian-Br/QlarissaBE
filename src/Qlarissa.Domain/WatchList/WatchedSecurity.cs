namespace Qlarissa.Domain.WatchList;

public sealed class WatchedSecurity
{
    public int SecurityId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public double PreviousDaysClosePrice { get; set; }
}