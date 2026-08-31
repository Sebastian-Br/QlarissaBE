using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application.Interfaces;

public interface ISecurityManager
{
    public Task<FluentResults.Result<string>> AddSecurityAsync(string securityTickerSymbol, CancellationToken cancellationToken);

    public Task<bool> SecurityExistsAsync(string securityTickerSymbol);
    public Task<IEnumerable<SearchResult>> SearchSecuritiesInternallyAsync(string userQuery, CancellationToken cancellationToken);
    public Task<IEnumerable<SearchResult>> SearchSecuritiesExternallyAsync(string userQuery, CancellationToken cancellationToken);
}