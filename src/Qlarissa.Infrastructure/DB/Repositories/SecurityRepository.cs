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

    public async Task AddSecurityAsync(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase security, CancellationToken cancellationToken)
    {
        PubliclyTradedSecurityBase dbEntity;

        if (security is Domain.Entities.Securities.Stock stock)
        {
            dbEntity = Stock.FromDomainEntity(stock);
        }
        else if (security is Domain.Entities.Securities.ETF etf)
        {
            dbEntity = ETF.FromDomainEntity(etf);
        }
        else if (security is Domain.Entities.Securities.CryptoCurrency cryptoCurrency)
        {
            dbEntity = CryptoCurrency.FromDomainEntity(cryptoCurrency);
        }
        else if (security is Domain.Entities.Securities.CurrencyPair currencyPair)
        {
            dbEntity = CurrencyPair.FromDomainEntity(currencyPair);
        }
        else
        {
            throw new NotImplementedException("Unsupported security type.");
        }

        _context.Set<PubliclyTradedSecurityBase>().Add(dbEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Securities.Base.PubliclyTradedSecurityBase?> GetSecurityAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _context.Set<PubliclyTradedSecurityBase>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (result == null)
        {
            return null;
        }

        return result switch
        {
            null => null,
            Stock stock => stock.ToDomainEntity(),
            ETF etf => etf.ToDomainEntity(),
            CryptoCurrency cryptoCurrency => cryptoCurrency.ToDomainEntity(),
            CurrencyPair currencyPair => currencyPair.ToDomainEntity(),
            _ => throw new NotImplementedException(
                $"Unsupported security type '{result.GetType().Name}'.")
        };
    }

    public async Task<IEnumerable<Domain.Entities.Securities.SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken)
    {
        var pattern = $"%{userQuery}%";

        var result = await _context.Set<PubliclyTradedSecurityBase>()
            .Where(s => EF.Functions.Like(s.Name, pattern) || EF.Functions.Like(s.Symbol, pattern))
            .Select(s => new Domain.Entities.Securities.SearchResult
            {
                Id = s.Id,
                Name = s.Name,
                Symbol = s.Symbol,
                SecurityType = (Domain.Entities.Securities.Base.SecurityType)s.SecurityType,
                Exchange = s.ExchangeName,
                ExchangeShortName = s.ExchangeShortName
            })
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<bool> SecurityExistsAsync(string tickerSymbol)
        => await _context.Set<PubliclyTradedSecurityBase>().AnyAsync(s => s.Symbol == tickerSymbol);
}