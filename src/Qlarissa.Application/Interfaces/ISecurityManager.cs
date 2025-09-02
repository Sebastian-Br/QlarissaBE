using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application.Interfaces;

public interface ISecurityManager
{
    public Task AddSecurityAsync(PubliclyTradedSecurityBase security);
}
