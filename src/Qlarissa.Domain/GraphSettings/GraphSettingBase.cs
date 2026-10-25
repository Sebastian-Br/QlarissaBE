namespace Qlarissa.Domain.GraphSettings;

public abstract class GraphSettingBase
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool LogarithmicScale { get; set; } = true;

    /// <summary>
    /// The distance in pixels from the hovered data point to the price card.
    /// Must be set by the class that inherits from this base class.
    /// </summary>
    public int PriceCardHoverDistance { get; set; }
}