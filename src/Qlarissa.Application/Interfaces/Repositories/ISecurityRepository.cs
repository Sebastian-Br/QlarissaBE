using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application.Interfaces.Repositories;

public interface ISecurityRepository
{
    /// <summary>
    /// Gets a security by its ID. Returns null if the security does not exist.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PubliclyTradedSecurityBase?> GetSecurityAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a security to the database. The currency of that security must already exist.
    /// </summary>
    /// <param name="security"></param>
    /// <returns></returns>
    Task AddSecurityAsync(PubliclyTradedSecurityBase security, CancellationToken cancellationToken);

    Task<bool> SecurityExistsAsync(string tickerSymbol);

    /// <summary>
    /// Searches for securities based on the user query. The search will be performed on the name and symbol of the security.
    /// </summary>
    /// <param name="userQuery">E.g. "Micros"</param>
    /// <param name="cancellationToken"></param>
    /// <returns>SearchResults, containing e.g. { Name = "Microsoft Corporation", ... }</returns>
    Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken);
}