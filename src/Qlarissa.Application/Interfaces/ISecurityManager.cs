using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application.Interfaces;

public interface ISecurityManager
{
    public Task<PubliclyTradedSecurityBase?> GetSecurityAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a security from an external API to the database. The currency of that security must already exist.
    /// </summary>
    /// <param name="securityTickerSymbol">The ticker symbol, e.g. "MSFT".</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns an Ok() result on success where the content is the added ticker symbol.</returns>
    public Task<FluentResults.Result<string>> AddSecurityAsync(string securityTickerSymbol, CancellationToken cancellationToken);

    public Task<bool> SecurityExistsAsync(string securityTickerSymbol);
    public Task<IEnumerable<SearchResult>> SearchSecuritiesInternallyAsync(string userQuery, CancellationToken cancellationToken);
    public Task<IEnumerable<SearchResult>> SearchSecuritiesExternallyAsync(string userQuery, CancellationToken cancellationToken);
}