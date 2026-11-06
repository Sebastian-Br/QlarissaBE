using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Entities.Base;
using ZiggyCreatures.Caching.Fusion;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SecurityRepository(ILogger<SecurityRepository> logger, IFusionCache cache, ApplicationDbContext context) : ISecurityRepository
{
    private readonly ILogger<SecurityRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IFusionCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));

    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddSecurityAsync(Domain.Securities.Base.PubliclyTradedSecurityBase security, CancellationToken cancellationToken)
    {
        PubliclyTradedSecurityBase dbEntity = PubliclyTradedSecurityBase.FromDomainEntity(security);
        _context.Set<PubliclyTradedSecurityBase>().Add(dbEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Domain.Securities.Base.PubliclyTradedSecurityBase?> GetSecurityFromCacheAsync(int id, CancellationToken cancellationToken)
    {
        return await _cache.GetOrSetAsync(
                Key.FullSecurityKey(id),
                async token => await GetSecurityAsync(id, token),
                tags: [Tag.Security(id)],
                token: cancellationToken
            );
    }

    public async Task<Domain.Securities.Base.PubliclyTradedSecurityBase?> GetSecurityAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _context.Set<PubliclyTradedSecurityBase>()
            .AsNoTracking()
            .Include(s => s.PriceHistory)
            .Include(s => s.DividendPayouts)
            .Include(s => s.Splits)
            .Include(s => s.Currency)
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (result == null)
        {
            return null;
        }

        return result.ToDomainEntity();
    }

    public async Task<Domain.Securities.Base.PubliclyTradedSecurityBase?> GetSecurityBasicAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _context.Set<PubliclyTradedSecurityBase>()
            .AsNoTracking()
            .Include(s => s.Currency)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (result == null)
        {
            return null;
        }

        return result.ToDomainEntity();
    }

    public async Task<ISecurityRepository.SecuritySymbolAndLastDataPointDate?> GetSecuritySymbolAndPriceHistoryLastDataPointDateAsync(int id)
    {
        var result = await _context.Set<PubliclyTradedSecurityBase>()
            .Where(s => s.Id == id)
            .Select(s => new ISecurityRepository.SecuritySymbolAndLastDataPointDate(s.Symbol, s.PriceHistoryLastDataPointDate))
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<IEnumerable<Domain.Securities.SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken)
    {
        var pattern = $"%{userQuery}%";

        var result = await _context.Set<PubliclyTradedSecurityBase>()
            .Where(s => EF.Functions.Like(s.Name, pattern) || EF.Functions.Like(s.Symbol, pattern))
            .Select(s => new Domain.Securities.SearchResult
            {
                Id = s.Id,
                Name = s.Name,
                Symbol = s.Symbol,
                SecurityType = (Domain.Securities.Base.SecurityType)s.SecurityType,
                Exchange = s.ExchangeName,
                ExchangeShortName = s.ExchangeShortName
            })
            .ToListAsync(cancellationToken);

        return result;
    }

    public Task<bool> SecurityExistsAsync(string tickerSymbol)
        => _context.Set<PubliclyTradedSecurityBase>().AnyAsync(s => s.Symbol == tickerSymbol);

    public async Task<Result> UpdateSecurityAsync(Domain.Securities.Base.PubliclyTradedSecurityBase security, CancellationToken cancellationToken)
    {
        var dbEntity = await _context.Set<PubliclyTradedSecurityBase>()
            .Include(s => s.PriceHistory)
            .Include(s => s.DividendPayouts)
            .Include(s => s.Splits)
            // The Currency will always remain the same - no need to include it.
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.Id == security.Id, cancellationToken);

        if (dbEntity == null)
        {
            return Result.Fail($"Security with ID {security.Id} not found.");
        }

        try
        {
            dbEntity.UpdateFromDomainEntity(security);
            await _context.SaveChangesAsync(cancellationToken);
            // Removes all security cache entries (full, basic), as well as cached watchedlists that watch this security.
            await cache.RemoveByTagAsync(Tag.Security(security.Id), token: cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating security with ID {SecurityId}", security.Id);
            return Result.Fail($"Error updating security with ID {security.Id}.");
        }
    }
}