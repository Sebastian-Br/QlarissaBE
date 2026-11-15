namespace Qlarissa.Domain.Portfolios;

public class HoldingEvent
{
    public int Id { get; set; }

    public HoldingEventType EventType { get; set; }

    public double Amount { get; set; }

    public DateOnly Date { get; set; }
}

public enum HoldingEventType
{
    Purchase,
    Sale,
}