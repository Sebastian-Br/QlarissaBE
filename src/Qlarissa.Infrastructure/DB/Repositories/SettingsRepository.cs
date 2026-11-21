using Microsoft.EntityFrameworkCore;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SettingsRepository(ApplicationDbContext context) : ISettingsRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Domain.Settings.Graph.GraphSettingBase> AddOrUpdateGraphSettingAsync(Domain.Settings.Graph.GraphSettingBase graphSetting, string userId)
    {
        var existingEntity = await _context.GraphSettings
                                .Include(x => x.Bounds)
                                .FirstOrDefaultAsync(x => x.UserId == userId && (int)x.DataSource == (int)graphSetting.DataSource);

        var incomingDbEntity = GraphSettingsBase.FromDomainEntity(graphSetting, userId);
        if (existingEntity is null)
        {
            await _context.GraphSettings.AddAsync(incomingDbEntity);
            await _context.SaveChangesAsync();

            return incomingDbEntity.ToDomainEntity();
        }

        existingEntity.UpdateFromDbEntity(incomingDbEntity);
        await _context.SaveChangesAsync();

        return existingEntity.ToDomainEntity();
    }

    public async Task<IEnumerable<Domain.Settings.Graph.GraphSettingBase>> GetGraphSettingsAsync(string userId)
    {
        var dbEntities = await _context.GraphSettings
                                    .Where(x => x.UserId == userId)
                                    .Include(x => x.Bounds)
                                    .ToListAsync();

        return dbEntities.Select(x => x.ToDomainEntity());
    }
}
