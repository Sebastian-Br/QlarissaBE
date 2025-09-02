using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Repositories.Interfaces;

public interface ISecurityRepository
{
    Task<T?> GetByIdAsync<T>(int id) where T : PubliclyTradedSecurityBase;
    Task<IEnumerable<T>> GetAllAsync<T>() where T : PubliclyTradedSecurityBase;

    Task<Currency?> GetBasicCurrencyAsync(string symbol);

    /// <summary>
    /// Adds a security to the database. The currency of that security must already exist.
    /// </summary>
    /// <param name="security"></param>
    /// <returns></returns>
    Task AddSecurityAsync(PubliclyTradedSecurityBase security);
    Task AddCurrencyAsync(Currency security);
}