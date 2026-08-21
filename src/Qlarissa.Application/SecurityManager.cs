using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application;

public sealed class SecurityManager(ISecurityRepository securityRepository) : ISecurityManager
{
    readonly ISecurityRepository _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));

    public async Task AddSecurityAsync(PubliclyTradedSecurityBase security)
    {
        await _securityRepository.AddSecurityAsync(security);
    }
}