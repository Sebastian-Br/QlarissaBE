using Qlarissa.Domain.Entities.Securities;

namespace Qlarissa.Infrastructure.Interfaces;

public interface IPyFinanceClient
{
    public Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken);
}