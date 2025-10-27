using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Repositories.Interfaces;

public interface ISecurityRepository
{
    Task<Domain.Entities.Securities.Currency?> GetCurrencyAsync(string symbol);

    Task AddCurrencyAsync(Currency security);


    /// <summary>
    /// Adds a security to the database. The currency of that security must already exist.
    /// </summary>
    /// <param name="security"></param>
    /// <returns></returns>
    Task AddSecurityAsync(PubliclyTradedSecurityBase security);
}