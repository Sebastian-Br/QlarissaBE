using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Domain.Settings.Graph;

namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public abstract class GraphSettingsBase<TGraphSettings> : IGraphSettingsBase where TGraphSettings : GraphSettingBase, new()
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

    public virtual TGraphSettings ToDomainEntity()
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

public class GraphSettingsBaseConfiguration<TEntity, TGraphSettings> : IEntityTypeConfiguration<TEntity>
    where TEntity : GraphSettingsBase<TGraphSettings>
    where TGraphSettings : GraphSettingBase, new()
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
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
            .HasForeignKey<TEntity>(x => x.BoundsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}