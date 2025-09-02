using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Repositories.Interfaces;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SecurityRepository(ILogger<SecurityRepository> logger, ApplicationDbContext context) : ISecurityRepository
{
    private readonly ILogger<SecurityRepository> _logger = logger;

    private readonly ApplicationDbContext _context = context;

    public async Task<T?> GetByIdAsync<T>(int id) where T : PubliclyTradedSecurityBase
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<Currency?> GetBasicCurrencyAsync(string symbol)
    {
        return await _context.Currencies.AsNoTracking().FirstAsync(c => c.Symbol == symbol);
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : PubliclyTradedSecurityBase
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task AddSecurityAsync(PubliclyTradedSecurityBase security)
    {
        _context.Set<PubliclyTradedSecurityBase>().Add(security);
        await _context.SaveChangesAsync();
    }

    public async Task AddCurrencyAsync(Currency security)
    {
        try
        {
            _context.Set<Currency>().Add(security);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Failed to add currency.");
        }
    }
}