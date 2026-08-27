using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.ExternalAPI;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application;

public sealed class SecurityManager(ISecurityRepository securityRepository, IMarketDataClient marketDataClient) : ISecurityManager
{
    readonly ISecurityRepository _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));

    readonly IMarketDataClient _marketDataClient = marketDataClient ?? throw new ArgumentNullException(nameof(marketDataClient));

    Task ISecurityManager.AddSecurityAsync(string securityTickerSymbol)
        => throw new NotImplementedException();

    Task<IEnumerable<SearchResult>> ISecurityManager.SearchSecuritiesExternallyAsync(string userQuery, CancellationToken cancellationToken)
        => _marketDataClient.SearchSecuritiesAsync(userQuery, cancellationToken);

    Task<IEnumerable<SearchResult>> ISecurityManager.SearchSecuritiesInternallyAsync(string userQuery, CancellationToken cancellationToken)
        => _securityRepository.SearchSecuritiesAsync(userQuery, cancellationToken);
}