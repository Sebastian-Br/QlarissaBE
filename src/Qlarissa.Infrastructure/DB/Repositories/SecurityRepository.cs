using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Entities;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SecurityRepository(ILogger<SecurityRepository> logger, ApplicationDbContext context) : ISecurityRepository
{
    private readonly ILogger<SecurityRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddSecurityAsync(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase security)
    {
        PubliclyTradedSecurityBase dbEntity;

        if (security is Domain.Entities.Securities.Stock)
        {
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

    public async Task<IEnumerable<Domain.Entities.Securities.SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken)
    {
        var pattern = $"%{userQuery}%";

        var result = await _context.Set<PubliclyTradedSecurityBase>()
            .Where(s => EF.Functions.Like(s.Name, pattern) || EF.Functions.Like(s.Symbol, pattern))
            .Select(s => new Domain.Entities.Securities.SearchResult
            {
                Name = s.Name,
                Symbol = s.Symbol,
                SecurityType = (Domain.Entities.Securities.Base.SecurityType)s.SecurityType,
                Exchange = s.ExchangeName,
                ExchangeShortName = s.ExchangeShortName,
                IsDbSearchResult = true
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}