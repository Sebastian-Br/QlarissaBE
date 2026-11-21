namespace Qlarissa.Domain.Settings.Graph;

public abstract class GraphSettingBase
{
    public int Id { get; set; }

    public GraphDataSource DataSource { get; set; }

    public PlotType PlotType { get; set; }

    public Bounds? Bounds { get; set; }

    public int TickCount { get; set; }

    public bool ShowHorizontalGridLines { get; set; }

    public bool ShowVerticalGridLines { get; set; }

    public bool LogarithmicScale { get; set; } = true;

    /// <summary>
    /// The distance in pixels from the hovered data point to the price card.
    /// </summary>
    public int ValueCardHoverDistance { get; set; }
}