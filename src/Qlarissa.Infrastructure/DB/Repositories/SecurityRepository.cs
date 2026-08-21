using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Application.Interfaces.Repositories;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SecurityRepository(ILogger<SecurityRepository> logger, ApplicationDbContext context) : ISecurityRepository
{
    private readonly ILogger<SecurityRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Domain.Entities.Securities.Currency?> GetCurrencyAsync(string symbol)
    {
        return (await _context.Currencies.AsNoTracking().FirstOrDefaultAsync(c => c.Symbol == symbol))?.ToDomainEntity();
    }

    public async Task AddSecurityAsync(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase security)
    {
        PubliclyTradedSecurityBase dbEntity;

        if (security is Domain.Entities.Securities.Stock)
        {
            var existingCurrency = await GetCurrencyAsync(security.Currency.Symbol);

            dbEntity = new Stock();
            Stock.FromDomainEntity(security, dbEntity);
            
        }
        else
        {
            throw new InvalidOperationException("Unsupported security type.");
        }

        _context.Set<PubliclyTradedSecurityBase>().Add(dbEntity);
        await _context.SaveChangesAsync();
    }

    public async Task AddCurrencyAsync(Domain.Entities.Securities.Currency security)
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