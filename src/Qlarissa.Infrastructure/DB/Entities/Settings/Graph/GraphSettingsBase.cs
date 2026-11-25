using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public abstract class GraphSettingsBase
{
    public string UserId { get; set; } = string.Empty;

    public GraphDataSource DataSource { get; set; }

    public PlotType PlotType { get; set; }

    // No need to include a PlotTypeTags collection here. The relationship may be n to n and they can change independently.

    /// <summary>
    /// GraphSettings tags will override PlotType tags when set.
    /// Purely display related settings can be stored as tags. They will never be accessed by the Back-End.
    /// </summary>
    public ICollection<GraphSettingsTag> Tags { get; set; } = [];

    // public int BoundsId { get; set; } redundant

    // public Bounds? Bounds { get; set; } -- move to tags

    // public int TickCount { get; set; } -- move to tags

    // public bool ShowHorizontalGridLines { get; set; } tags

    // public bool ShowVerticalGridLines { get; set; } tags

    // public bool LogarithmicScale { get; set; } = true; tags

    // public int ValueCardHoverDistance { get; set; } tags

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
            PlotType = (Domain.Settings.Graph.PlotType)PlotType,
            Tags = Tags.ToDictionary(x => x.Key, y => y.Value)
        };
    }

    protected virtual void FromDomainEntity(TGraphSettings domainEntity, string userId)
    {
        UserId = userId;
        DataSource = (GraphDataSource)domainEntity.DataSource;
        PlotType = (PlotType)domainEntity.PlotType;
        Tags = GraphSettingsTag.FromDomainEntities(domainEntity.Tags, userId, (GraphDataSource)domainEntity.DataSource);
    }

    public override void UpdateFromDbEntity(GraphSettingsBase dbEntity)
    {
        PlotType = dbEntity.PlotType;

        // Tags are updated separately.

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
        // querying by unique UserId x DataSource
        builder.HasKey(x => new { x.UserId, x.DataSource });

        builder.HasMany(x => x.Tags)
                .WithOne()
                .HasForeignKey(x => new
                {
                    x.UserId,
                    x.GraphSetting
                })
                .OnDelete(DeleteBehavior.Cascade);

        //builder.ToTable("GraphSettings");
    }
}