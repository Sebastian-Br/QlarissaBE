using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public abstract class GraphSettingsBase
{
    public string UserId { get; set; } = string.Empty;

    public GraphDataSource DataSource { get; set; }

    public int BoundsId { get; set; }

    public Bounds? Bounds { get; set; }

    public int TickCount { get; set; }

    public bool ShowHorizontalGridLines { get; set; }

    public bool ShowVerticalGridLines { get; set; }

    public bool LogarithmicScale { get; set; } = true;

    public int ValueCardHoverDistance { get; set; }

    public static GraphSettingsBase FromDomainEntity(Domain.Settings.Graph.GraphSettingBase domainEntity, string userId) => domainEntity switch
    {
        Domain.Settings.Graph.SecurityGraphSettings securityGraphSettings => new SecurityGraphSettings(securityGraphSettings, userId),
        Domain.Settings.Graph.DividendPayoutGraphSettings dividendPayoutGraphSettings => new DividendPayoutGraphSettings(dividendPayoutGraphSettings, userId),
        _ => throw new NotSupportedException($"Unsupported graph settings type: {domainEntity.GetType().Name}")
    };

    public abstract Domain.Settings.Graph.GraphSettingBase ToDomainEntity();

    public abstract void UpdateFromDbEntity(GraphSettingsBase dbEntity);
}

/// <summary>
/// Used for mapping.
/// </summary>
/// <typeparam name="TGraphSettings">The domain GraphSettings type.</typeparam>
public abstract class GraphSettingsBase<TGraphSettings> : GraphSettingsBase 
                        where TGraphSettings : Domain.Settings.Graph.GraphSettingBase, new()
{
    public override TGraphSettings ToDomainEntity()
    {
        return new TGraphSettings
        {
            DataSource = (Domain.Settings.Graph.GraphDataSource)DataSource,
            Bounds = Bounds?.ToDomainEntity(),
            TickCount = TickCount,
            ShowHorizontalGridLines = ShowHorizontalGridLines,
            ShowVerticalGridLines = ShowVerticalGridLines,
            LogarithmicScale = LogarithmicScale,
            ValueCardHoverDistance = ValueCardHoverDistance
        };
    }

    protected virtual void FromDomainEntity(TGraphSettings domainEntity, string userId)
    {
        UserId = userId;
        DataSource = (GraphDataSource)domainEntity.DataSource;
        Bounds = Bounds.FromDomainEntity(domainEntity.Bounds);
        TickCount = domainEntity.TickCount;
        ShowHorizontalGridLines = domainEntity.ShowHorizontalGridLines;
        ShowVerticalGridLines = domainEntity.ShowVerticalGridLines;
        LogarithmicScale = domainEntity.LogarithmicScale;
        ValueCardHoverDistance = domainEntity.ValueCardHoverDistance;
    }

    public override void UpdateFromDbEntity(GraphSettingsBase dbEntity)
    {
        Bounds = Bounds?.UpdateFromDbEntity(dbEntity.Bounds)
                ?? (dbEntity.Bounds is null
                 ? null
                 : new Bounds().UpdateFromDbEntity(dbEntity.Bounds));

        TickCount = dbEntity.TickCount;
        ShowHorizontalGridLines = dbEntity.ShowHorizontalGridLines;
        ShowVerticalGridLines = dbEntity.ShowVerticalGridLines;
        LogarithmicScale = dbEntity.LogarithmicScale;
        ValueCardHoverDistance = dbEntity.ValueCardHoverDistance;

        if (this is SecurityGraphSettings securityGraphSettingsEntity && dbEntity is SecurityGraphSettings incomingSecurityGraphSettings)
        {
            securityGraphSettingsEntity.UpdateFromDbEntity(incomingSecurityGraphSettings);
        }
        else if (this is DividendPayoutGraphSettings dividendPayoutGraphSettingsEntity 
            && dbEntity is DividendPayoutGraphSettings incomingDividendGraphSettings)
        {
            dividendPayoutGraphSettingsEntity.UpdateFromDbEntity(incomingDividendGraphSettings);
        }
        else
        {
            throw new NotImplementedException("This GraphSettings type is unsupported " + dbEntity.GetType());
        }
    }
}

public class GraphSettingsBaseConfiguration : IEntityTypeConfiguration<GraphSettingsBase>
{
    public void Configure(EntityTypeBuilder<GraphSettingsBase> builder)
    {
        // querying by UserId x GraphType
        builder.HasKey(x => new { x.UserId, x.DataSource });

        // querying by UserId
        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.Bounds)
               .WithOne()
               .HasForeignKey<GraphSettingsBase>(x => x.BoundsId)
               .OnDelete(DeleteBehavior.Cascade);

        //builder.ToTable("GraphSettings");
    }
}