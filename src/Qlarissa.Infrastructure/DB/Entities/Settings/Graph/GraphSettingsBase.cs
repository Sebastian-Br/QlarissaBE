using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public abstract class GraphSettingsBase
{
    public string UserId { get; set; } = string.Empty;

    public Domain.Settings.Graph.GraphDataSource DataSource { get; set; }

    public Domain.Settings.Graph.PlotType PlotType { get; set; }

    public Bounds? Bounds { get; set; }

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
            DataSource = DataSource,
            PlotType = PlotType,
            Bounds = Bounds?.ToDomainEntity()
        };
    }

    protected virtual void FromDomainEntity(TGraphSettings domainEntity, string userId)
    {
        UserId = userId;
        DataSource = domainEntity.DataSource;
        PlotType = domainEntity.PlotType;
        Bounds = Bounds.FromDomainEntity(domainEntity.Bounds);
    }

    public override void UpdateFromDbEntity(GraphSettingsBase dbEntity)
    {
        PlotType = dbEntity.PlotType;

        Bounds = dbEntity.Bounds is null
            ? null
            : new Bounds
            {
                StartDate = dbEntity.Bounds.StartDate,
                EndDate = dbEntity.Bounds.EndDate,
                Start = dbEntity.Bounds.Start,
                End = dbEntity.Bounds.End
            };

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

        builder.OwnsOne(x => x.Bounds, bounds =>
        {
            bounds.Property(x => x.StartDate).HasColumnName("Bounds_StartDate");
            bounds.Property(x => x.EndDate).HasColumnName("Bounds_EndDate");
            bounds.Property(x => x.Start).HasColumnName("Bounds_Start");
            bounds.Property(x => x.End).HasColumnName("Bounds_End");
        });
        builder.Navigation(x => x.Bounds).IsRequired(false);
    }
}