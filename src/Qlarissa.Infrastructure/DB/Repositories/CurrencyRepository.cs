using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Application.Interfaces.Repositories;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class CurrencyRepository(ILogger<CurrencyRepository> logger, ApplicationDbContext context) : ICurrencyRepository
{
    private readonly ILogger<CurrencyRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Domain.Entities.Currency?> GetCurrencyAsync(string symbol)
    {
        return (await _context.Currencies.AsNoTracking().FirstOrDefaultAsync(c => c.Symbol == symbol))?.ToDomainEntity();
    }

    public async Task<IEnumerable<Domain.Entities.Currency>> GetCurrenciesAsync()
    {
        return await _context.Currencies.AsNoTracking().Select(c => c.ToDomainEntity()).ToListAsync();
    }

    public async Task AddCurrencyAsync(Domain.Entities.Currency security)
    {
        try
        {
            var dbEntity = Currency.FromDomainEntity(security);
            _context.Set<Currency>().Add(dbEntity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Failed to add currency.");
        }
    }
}