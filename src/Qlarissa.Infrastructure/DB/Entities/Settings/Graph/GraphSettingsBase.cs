using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public abstract class GraphSettingsBase
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public GraphType GraphType { get; set; }

    public int BoundsId { get; set; }

    public Bounds? Bounds { get; set; }

    public int TickCount { get; set; }

    public bool ShowXAxisGridLines { get; set; }

    public bool ShowYAxisGridLines { get; set; }

    public bool LogarithmicScale { get; set; } = true;

    public int PriceCardHoverDistance { get; set; }

    public static GraphSettingsBase FromDomainEntity(Domain.Settings.Graph.GraphSettingBase domainEntity, string userId) => domainEntity switch
    {
        Domain.Settings.Graph.SecurityGraphSettings securityGraphSettings => new SecurityGraphSettings(securityGraphSettings, userId),
        Domain.Settings.Graph.DividendPayoutGraphSettings dividendPayoutGraphSettings => new DividendPayoutGraphSettings(dividendPayoutGraphSettings, userId),
        _ => throw new NotSupportedException($"Unsupported graph settings type: {domainEntity.GetType().Name}")
    };

    public abstract Domain.Settings.Graph.GraphSettingBase ToDomainEntity();
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
            Id = Id,
            GraphType = (Domain.Settings.Graph.GraphType)GraphType,
            Bounds = Bounds?.ToDomainEntity(),
            TickCount = TickCount,
            ShowXAxisGridLines = ShowXAxisGridLines,
            ShowYAxisGridLines = ShowYAxisGridLines,
            LogarithmicScale = LogarithmicScale,
            ValueCardHoverDistance = PriceCardHoverDistance
        };
    }

    public virtual void FromDomainEntity(TGraphSettings domainEntity, string userId)
    {
        Id = domainEntity.Id;
        UserId = userId;
        GraphType = (GraphType)domainEntity.GraphType;
        Bounds = Bounds.FromDomainEntity(domainEntity.Bounds);
        TickCount = domainEntity.TickCount;
        ShowXAxisGridLines = domainEntity.ShowXAxisGridLines;
        ShowYAxisGridLines = domainEntity.ShowYAxisGridLines;
        LogarithmicScale = domainEntity.LogarithmicScale;
        PriceCardHoverDistance = domainEntity.ValueCardHoverDistance;
    }
}

public class GraphSettingsBaseConfiguration : IEntityTypeConfiguration<GraphSettingsBase>
{
    public void Configure(EntityTypeBuilder<GraphSettingsBase> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // querying by UserId
        builder.HasIndex(x => x.UserId);

        // querying by UserId x Id to update
        builder.HasIndex(x => new { x.Id, x.UserId });

        // querying by UserId x GraphType
        builder.HasIndex(x => new { x.UserId, x.GraphType });

        builder.HasOne(x => x.Bounds)
               .WithOne()
               .HasForeignKey<GraphSettingsBase>(x => x.BoundsId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("GraphSettings");
    }
}