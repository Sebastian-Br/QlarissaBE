using System.Data;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Qlarissa.Application.Repositories;
using Qlarissa.Infrastructure.DB.Entities.Settings.Graph;
using Qlarissa.Infrastructure.DB.Entities.Settings;

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
            .FirstAsync(x => x.UserId == userId && (int)x.DataSource == (int)graphSetting);

        return dbEntity.ToDomainEntity();
    }

    public async Task<IReadOnlyList<Qlarissa.Domain.Settings.DisplaySetting>> GetDisplaySettingsAsync(
        string userId,
        Qlarissa.Domain.Settings.DeviceProfile deviceProfile)
    {
        var storedSettings = await _context.DisplaySettings
            .AsNoTracking()
            .Where(setting => setting.UserId == userId && setting.DeviceProfile == deviceProfile)
            .Select(setting => new { setting.Key, setting.Value })
            .ToListAsync();

        return storedSettings
            .Select(setting => new Qlarissa.Domain.Settings.DisplaySetting(setting.Key, setting.Value))
            .ToList();
    }

    public async Task<FluentResults.Result> SyncDisplaySettingsAsync(
        string userId,
        Qlarissa.Domain.Settings.DeviceProfile deviceProfile,
        IReadOnlyCollection<Qlarissa.Domain.Settings.DisplaySettingChange> changes,
        int maxRowsPerUser)
    {
        if (changes.Count == 0)
            return FluentResults.Result.Ok();

        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var userSettings = await _context.DisplaySettings
            .Where(setting => setting.UserId == userId)
            .ToListAsync();
        var profileSettings = userSettings
            .Where(setting => setting.DeviceProfile == deviceProfile)
            .ToDictionary(setting => setting.Key, StringComparer.Ordinal);

        foreach (var change in changes)
        {
            if (change.Value is null)
            {
                if (profileSettings.Remove(change.Key, out var removed))
                {
                    _context.DisplaySettings.Remove(removed);
                    userSettings.Remove(removed);
                }
                continue;
            }

            if (profileSettings.TryGetValue(change.Key, out var existing))
            {
                existing.Value = change.Value;
                continue;
            }

            var added = new UserDisplaySetting
            {
                UserId = userId,
                DeviceProfile = deviceProfile,
                Key = change.Key,
                Value = change.Value
            };
            profileSettings.Add(change.Key, added);
            userSettings.Add(added);
            _context.DisplaySettings.Add(added);
        }

        if (userSettings.Count > maxRowsPerUser)
            return FluentResults.Result.Fail(
                $"Display settings quota exceeded. A user may store at most {maxRowsPerUser} settings across all device profiles.");

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return FluentResults.Result.Ok();
    }
}
