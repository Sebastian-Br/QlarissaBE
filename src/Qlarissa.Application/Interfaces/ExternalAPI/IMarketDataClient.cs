using Qlarissa.Domain.Entities.Securities;

namespace Qlarissa.Application.Interfaces.ExternalAPI;

public interface IMarketDataClient
{
    public Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken);

    public Task<Domain.Entities.Securities.Base.PubliclyTradedSecurityBase> GetSecurityAsync(string tickerSymbol, CancellationToken cancellationToken);
}