using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public sealed class PlotSettingsTag
{
    public string UserId { get; set; } = string.Empty;

    public PlotType PlotType { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}

public class PlotSettingsTagConfiguration : IEntityTypeConfiguration<PlotSettingsTag>
{
    public void Configure(EntityTypeBuilder<PlotSettingsTag> builder)
    {
        builder.HasKey(x => new { x.UserId, x.PlotType, x.Key });

        //builder.ToTable("PlotSettingsTags");
    }
}