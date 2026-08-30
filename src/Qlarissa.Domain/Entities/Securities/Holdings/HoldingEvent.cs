namespace Qlarissa.Domain.Entities.Securities.Holdings;

public class HoldingEvent
{
    public int Id { get; set; }

    public HoldingEventType EventType { get; set; }

    public int Quantity { get; set; }

    public DateOnly Date { get; set; }
}

public enum HoldingEventType
{
    Buy,
    Sell,
}