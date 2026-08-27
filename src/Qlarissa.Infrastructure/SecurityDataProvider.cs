using Qlarissa.Application.Interfaces.MarketData;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Infrastructure.Interfaces;

namespace Qlarissa.Infrastructure;

public class SecurityDataProvider(ISecurityRepository securityRepository, IPyFinanceClient pyFinanceClient) : ISecurityDataProvider
{
    readonly ISecurityRepository _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));

    readonly IPyFinanceClient _pyFinanceClient = pyFinanceClient ?? throw new ArgumentNullException(nameof(pyFinanceClient));
    public Task AddSecurityAsync(string securityTickerSymbol)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SearchResult>> SearchSecuritiesExternallyAsync(string userQuery, CancellationToken cancellationToken)
        => _pyFinanceClient.SearchSecuritiesAsync(userQuery, cancellationToken);

    public Task<IEnumerable<SearchResult>> SearchSecuritiesInternallyAsync(string userQuery, CancellationToken cancellationToken)
        => _securityRepository.SearchSecuritiesAsync(userQuery, cancellationToken);
}