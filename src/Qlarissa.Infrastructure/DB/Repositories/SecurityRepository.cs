using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Repositories.Interfaces;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SecurityRepository(ILogger<SecurityRepository> logger, ApplicationDbContext context) : ISecurityRepository
{
    private readonly ILogger<SecurityRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Domain.Entities.Securities.Currency?> GetCurrencyAsync(string symbol)
    {
        return (await _context.Currencies.AsNoTracking().FirstOrDefaultAsync(c => c.Symbol == symbol))?.ToDomainEntity();
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