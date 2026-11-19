namespace Qlarissa.Domain.Portfolios;

public class Holding : HoldingMinimal
{
    public IEnumerable<HoldingEvent> HoldingEvents { get; set; } = [];
}