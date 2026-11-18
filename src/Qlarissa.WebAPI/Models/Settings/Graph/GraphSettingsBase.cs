using System.Text.Json.Serialization;

namespace Qlarissa.WebAPI.Models.Settings.Graph;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "GraphType")]
[JsonDerivedType(typeof(SecurityGraphSettings), (int)GraphType.Security)]
[JsonDerivedType(typeof(DividendPayoutGraphSettings), (int)GraphType.DividendPayouts)]
public abstract class GraphSettingsBase
{
    public GraphType GraphType { get; set; }

    public Bounds? Bounds { get; set; }

    public int TickCount { get; set; }

    public bool ShowXAxisGridLines { get; set; }

    public bool ShowYAxisGridLines { get; set; }

    public bool LogarithmicScale { get; set; }

    public int ValueCardHoverDistance { get; set; }

    public static GraphSettingsBase FromDomainEntity(Domain.Settings.Graph.GraphSettingBase domainEntity) => domainEntity switch
    {
        Domain.Settings.Graph.SecurityGraphSettings securityGraphSettings => new SecurityGraphSettings(securityGraphSettings),
        Domain.Settings.Graph.DividendPayoutGraphSettings dividendPayoutGraphSettings => new DividendPayoutGraphSettings(dividendPayoutGraphSettings),
        _ => throw new NotSupportedException($"Unsupported graph settings type: {domainEntity.GetType().Name}")
    };

    public abstract Domain.Settings.Graph.GraphSettingBase ToDomainEntity();
}

public class GraphSettingsBase<TGraphSettings> : GraphSettingsBase where TGraphSettings : Domain.Settings.Graph.GraphSettingBase, new()
{
    public override TGraphSettings ToDomainEntity() => new()
    {
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
        GraphType = (GraphType)domainEntity.GraphType;
        Bounds = Bounds.FromDomainEntity(domainEntity.Bounds);
        TickCount = domainEntity.TickCount;
        ShowXAxisGridLines = domainEntity.ShowXAxisGridLines;
        ShowYAxisGridLines = domainEntity.ShowYAxisGridLines;
        LogarithmicScale = domainEntity.LogarithmicScale;
        ValueCardHoverDistance = domainEntity.ValueCardHoverDistance;
    }
}
