namespace Qlarissa.Domain.Settings.Graph;

public abstract class GraphSettingBase
{
    public int Id { get; set; }

    public GraphDataSource DataSource { get; set; }

    public PlotType PlotType { get; set; }

    public Bounds? Bounds { get; set; }
}