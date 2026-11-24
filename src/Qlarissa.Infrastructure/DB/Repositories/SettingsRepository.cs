using Microsoft.EntityFrameworkCore;
using Qlarissa.Application.Repositories;
using Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SettingsRepository(ApplicationDbContext context) : ISettingsRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddOrUpdateGraphSettingAsync(string userId, Domain.Settings.Graph.GraphSettingBase graphSetting)
    {
        var existingEntity = await _context.GraphSettings
                                .FirstOrDefaultAsync(x => x.UserId == userId && (int)x.DataSource == (int)graphSetting.DataSource);

        var incomingDbEntity = GraphSettingsBase.FromDomainEntity(graphSetting, userId);
        if (existingEntity is null)
        {
            _context.GraphSettings.Add(incomingDbEntity);
            await _context.SaveChangesAsync();
            return;
        }

        existingEntity.UpdateFromDbEntity(incomingDbEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Domain.Settings.Graph.GraphSettingBase>> GetGraphSettingsAsync(string userId)
    {
        var dbEntities = await _context.GraphSettings
                                    .Where(x => x.UserId == userId)
                                    .ToListAsync();

        return dbEntities.Select(x => x.ToDomainEntity());
    }

    public async Task<Domain.Settings.Graph.GraphSettingBase> GetGraphSettingAsync(string userId, Domain.Settings.Graph.GraphDataSource graphSetting)
    {
        var dbEntity = await _context.GraphSettings
            .Include(x => x.Tags)
            .FirstAsync(x => x.UserId == userId && (int)x.DataSource == (int)graphSetting);

        return dbEntity.ToDomainEntity();
    }

    public async Task<Domain.Settings.SettingsTags<Domain.Settings.Graph.PlotType>> GetPlotSettingAsync(string userId, Domain.Settings.Graph.PlotType plotType)
    {
        var tags = await _context.PlotSettingsTags
            .Where(x => x.UserId == userId && x.PlotType == (PlotType)plotType)
            .ToDictionaryAsync(x => x.Key, x => x.Value);

        return new Domain.Settings.SettingsTags<Domain.Settings.Graph.PlotType>((int)plotType, tags);
    }

    public async Task AddOrUpdateGraphSettingsTags(string userId, Domain.Settings.SettingsTags<Domain.Settings.Graph.GraphSettingBase> tags)
    {
        var graphSetting = (GraphDataSource)tags.SettingsTypeId;
        var existingTags = await _context.GraphSettingsTags
            .Where(x => x.UserId == userId && x.GraphSetting == graphSetting)
            .ToListAsync();
        List<GraphSettingsTag> newTags = [];

        foreach (var tag in tags.Tags)
        {
            var existingTag = existingTags.FirstOrDefault(x => x.Key == tag.Key);
            if (existingTag is null)
            {
                newTags.Add(new GraphSettingsTag
                {
                    UserId = userId,
                    GraphSetting = graphSetting,
                    Key = tag.Key,
                    Value = tag.Value
                });
            }
            else
            {
                existingTag.Value = tag.Value;
            }
        }

        _context.GraphSettingsTags.AddRange(newTags);
        await _context.SaveChangesAsync();
    }

    public async Task AddOrUpdatePlotSettingsTags(string userId, Domain.Settings.SettingsTags<Domain.Settings.Graph.PlotType> tags)
    {
        var plotType = (PlotType)tags.SettingsTypeId;
        var existingTags = await _context.PlotSettingsTags
            .Where(x => x.UserId == userId && x.PlotType == plotType)
            .ToListAsync();
        List<PlotSettingsTag> newTags = [];

        foreach (var tag in tags.Tags)
        {
            var existingTag = existingTags.FirstOrDefault(x => x.Key == tag.Key);
            if (existingTag is null)
            {
                newTags.Add(new PlotSettingsTag
                {
                    UserId = userId,
                    PlotType = plotType,
                    Key = tag.Key,
                    Value = tag.Value
                });
            }
            else
            {
                existingTag.Value = tag.Value;
            }
        }

        _context.PlotSettingsTags.AddRange(newTags);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveGraphSettingsTags(string userId, Domain.Settings.SettingsTags<Domain.Settings.Graph.GraphSettingBase> tags)
    {
        if (tags.Tags.Count == 0)
        {
            return;
        }

        var graphSetting = (GraphDataSource)tags.SettingsTypeId;
        var keys = tags.Tags.Keys.ToArray();
        await _context.GraphSettingsTags
            .Where(x => x.UserId == userId && x.GraphSetting == graphSetting && keys.Contains(x.Key))
            .ExecuteDeleteAsync();
    }

    public async Task RemovePlotSettingsTags(string userId, Domain.Settings.SettingsTags<Domain.Settings.Graph.PlotType> tags)
    {
        if (tags.Tags.Count == 0)
        {
            return;
        }

        var plotType = (PlotType)tags.SettingsTypeId;
        var keys = tags.Tags.Keys.ToArray();
        await _context.PlotSettingsTags
            .Where(x => x.UserId == userId && x.PlotType == plotType && keys.Contains(x.Key))
            .ExecuteDeleteAsync();
    }
}
