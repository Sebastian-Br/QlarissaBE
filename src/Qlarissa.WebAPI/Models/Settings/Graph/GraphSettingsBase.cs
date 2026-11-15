namespace Qlarissa.WebAPI.Models.Settings.Graph;

public class GraphSettingsBase<TGraphSettings> : IGraphSettingsBase where TGraphSettings : Domain.Settings.Graph.GraphSettingBase, new()
{
    public int Id { get; set; }

    public GraphType GraphType { get; set; }

    public Bounds? Bounds { get; set; }

    public int TickCount { get; set; }

    public bool ShowXAxisGridLines { get; set; }

    public bool ShowYAxisGridLines { get; set; }

    public bool LogarithmicScale { get; set; }

    public int ValueCardHoverDistance { get; set; }

    internal virtual TGraphSettings ToDomainEntity() => new()
    {
        Id = Id,
        GraphType = (Domain.Settings.Graph.GraphType)GraphType,
        Bounds = Bounds?.ToDomainEntity(),
        TickCount = TickCount,
        ShowXAxisGridLines = ShowXAxisGridLines,
        ShowYAxisGridLines = ShowYAxisGridLines,
        LogarithmicScale = LogarithmicScale,
        ValueCardHoverDistance = ValueCardHoverDistance
    };

    internal virtual void FromDomainEntity(TGraphSettings domainEntity)
    {
        Id = domainEntity.Id;
        GraphType = (GraphType)domainEntity.GraphType;
        Bounds = Bounds.FromDomainEntity(domainEntity.Bounds);
        TickCount = domainEntity.TickCount;
        ShowXAxisGridLines = domainEntity.ShowXAxisGridLines;
        ShowYAxisGridLines = domainEntity.ShowYAxisGridLines;
        LogarithmicScale = domainEntity.LogarithmicScale;
        ValueCardHoverDistance = domainEntity.ValueCardHoverDistance;
    }
}
