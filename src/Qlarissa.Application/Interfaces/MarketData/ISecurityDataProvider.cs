using Qlarissa.Domain.Entities.Securities;

namespace Qlarissa.Application.Interfaces.MarketData;

public interface ISecurityDataProvider
{
    public Task AddSecurityAsync(string securityTickerSymbol);

    /// <summary>
    /// Searches for securities externally (via an API) based on the provided user query.
    /// </summary>
    /// <param name="userQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The SearchResults. These may overlap with internally found securities. The FE must filter them to not show duplicates in the UI.</returns>
    public Task<IEnumerable<SearchResult>> SearchSecuritiesExternallyAsync(string userQuery, CancellationToken cancellationToken);

    /// <summary>
    /// Searches for securities internally (via the database) based on the provided user query.
    /// </summary>
    /// <param name="userQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The SearchResults. These may overlap with externally found securities. The FE must use the result of this query to give priority to internally found securities.</returns>
    public Task<IEnumerable<SearchResult>> SearchSecuritiesInternallyAsync(string userQuery, CancellationToken cancellationToken);
}