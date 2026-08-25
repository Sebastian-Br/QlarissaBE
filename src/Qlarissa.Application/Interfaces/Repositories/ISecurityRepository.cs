using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application.Interfaces.Repositories;

public interface ISecurityRepository
{
    /// <summary>
    /// Adds a security to the database. The currency of that security must already exist.
    /// </summary>
    /// <param name="security"></param>
    /// <returns></returns>
    Task AddSecurityAsync(PubliclyTradedSecurityBase security);
}