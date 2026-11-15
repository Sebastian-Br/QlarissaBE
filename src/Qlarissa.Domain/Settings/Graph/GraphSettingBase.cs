namespace Qlarissa.Domain.Settings.Graph;

public abstract class GraphSettingBase
{
    public int Id { get; set; }

    public GraphType GraphType { get; set; }

    public Bounds? Bounds { get; set; }

    public int TickCount { get; set; }

    public bool ShowYAxisGridLines { get; set; }

    public bool ShowXAxisGridLines { get; set; }

    public bool LogarithmicScale { get; set; } = true;

    /// <summary>
    /// The distance in pixels from the hovered data point to the price card.
    /// </summary>
    public int ValueCardHoverDistance { get; set; }
}