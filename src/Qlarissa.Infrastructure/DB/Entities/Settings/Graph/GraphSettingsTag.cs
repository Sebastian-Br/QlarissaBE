using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public sealed class GraphSettingsTag
{
    public string UserId { get; set; } = string.Empty;

    public GraphDataSource GraphSetting { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public static List<GraphSettingsTag> FromDomainEntities(Dictionary<string, string> domainTags, string userId, GraphDataSource graphSettingsType)
    {
        List<GraphSettingsTag> result = [];

        foreach (KeyValuePair<string, string> kvp in domainTags)
        {
            result.Add(
                new GraphSettingsTag()
                {
                    UserId = userId,
                    GraphSetting = graphSettingsType,
                    Key = kvp.Key,
                    Value = kvp.Value
                }
            );
        }

        return result;
    }
}

public class GraphSettingsTagConfiguration : IEntityTypeConfiguration<GraphSettingsTag>
{
    public void Configure(EntityTypeBuilder<GraphSettingsTag> builder)
    {
        builder.HasKey(x => new { x.UserId, x.GraphSetting, x.Key });

        //builder.ToTable("GraphSettingsTags");
    }
}