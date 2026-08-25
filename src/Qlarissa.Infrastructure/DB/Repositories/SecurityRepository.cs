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
}