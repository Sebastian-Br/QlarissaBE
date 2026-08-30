namespace Qlarissa.Domain.Entities.Securities.MarketData;

public sealed class Split
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public double SplitRatio { get; set; }
}