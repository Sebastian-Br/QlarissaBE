using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Settings;

public sealed class UserDisplaySetting
{
    public string UserId { get; set; } = string.Empty;
    public Domain.Settings.DeviceProfile DeviceProfile { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public sealed class UserDisplaySettingConfiguration : IEntityTypeConfiguration<UserDisplaySetting>
{
    public void Configure(EntityTypeBuilder<UserDisplaySetting> builder)
    {
        builder.ToTable("DisplaySettings");
        builder.HasKey(setting => new { setting.UserId, setting.DeviceProfile, setting.Key });
        builder.Property(setting => setting.Key).HasMaxLength(128).IsRequired();
        builder.Property(setting => setting.Value).HasMaxLength(4000).IsRequired();
    }
}
