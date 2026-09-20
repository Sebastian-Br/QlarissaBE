using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SecurityRepository(ILogger<SecurityRepository> logger, ApplicationDbContext context) : ISecurityRepository
{
    private readonly ILogger<SecurityRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddSecurityAsync(Domain.Securities.Base.PubliclyTradedSecurityBase security, CancellationToken cancellationToken)
    {
        PubliclyTradedSecurityBase dbEntity = PubliclyTradedSecurityBase.FromDomainEntity(security);
        _context.Set<PubliclyTradedSecurityBase>().Add(dbEntity);
        await _context.SaveChangesAsync(cancellationToken);
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
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating security with ID {SecurityId}", security.Id);
            return Result.Fail($"Error updating security with ID {security.Id}.");
        }
    }

    public async Task<Domain.WatchedSecurityMinimal> IsSecurityWatchedAsync(int securityId, string userId)
    {
        var result = await _context.WatchedSecurities
                .Where(w => w.SecurityId == securityId && w.WatchedByUserId == userId)
                .Select(w => new Domain.WatchedSecurityMinimal
                {
                    IsWatched = true,
                    IsOnPrimaryWatchlist = w.IsPrimaryWatchlist
                })
                .FirstOrDefaultAsync();

        if (result == null)
        {
            return new Domain.WatchedSecurityMinimal
            {
                IsWatched = false,
                IsOnPrimaryWatchlist = false
            };
        }

        return result;
    }

    public async Task<Result> WatchSecurityAsync(int securityId, string userId, bool isPrimaryWatchlist)
    {
        try
        {
            await _context.WatchedSecurities.AddAsync(new WatchedSecurity
            {
                SecurityId = securityId,
                WatchedByUserId = userId,
                IsPrimaryWatchlist = isPrimaryWatchlist
            });

            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error watching security with ID {SecurityId} for user {UserId}", securityId, userId);
            return Result.Fail("This security is already watched.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error watching security with ID {SecurityId} for user {UserId}", securityId, userId);
            return Result.Fail("Unknown error watching this security.");
        }
    }

    public async Task<Result> UnwatchSecurityAsync(int securityId, string userId)
    {
        try
        {
            var deleted = await _context.WatchedSecurities.Where(w => w.SecurityId == securityId && w.WatchedByUserId == userId).ExecuteDeleteAsync();

            if (deleted == 0)
            {
                return Result.Fail($"Security with ID {securityId} is not watched by this user.");
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unwatching security with ID {SecurityId} for user {UserId}", securityId, userId);
            return Result.Fail("Unknown error unwatching this security.");
        }
    }

    public async Task<IEnumerable<Domain.WatchList>> GetWatchlistsAsync(string userId, CancellationToken cancellationToken)
    {
        var watchedSecurities = await _context.WatchedSecurities
            .Where(w => w.WatchedByUserId == userId)
            .Select(w => new
            {
                w.IsPrimaryWatchlist,
                WatchedSecurity = new Domain.WatchedSecurity()
                {
                    SecurityId = w.SecurityId,
                    Name = w.Security.Name,
                    Symbol = w.Security.Symbol,
                    PreviousDaysClosePrice = w.Security.PriceHistory
                        .OrderByDescending(ph => ph.Id)
                        .Select(ph => ph.Average)
                        .FirstOrDefault()
                }
            })
            .ToListAsync(cancellationToken);

        return [
            new Domain.WatchList
            {
                IsPrimary = true,
                WatchedSecurities = watchedSecurities
                    .Where(w => w.IsPrimaryWatchlist)
                    .Select(w => w.WatchedSecurity)
            },
            new Domain.WatchList
            {
                IsPrimary = false,
                WatchedSecurities = watchedSecurities
                    .Where(w => !w.IsPrimaryWatchlist)
                    .Select(w => w.WatchedSecurity)
            }
            ];
    }
}