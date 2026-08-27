using Qlarissa.Application.Interfaces.ExternalAPI;
using System.Net.Http.Json;

namespace Qlarissa.Infrastructure.PyFinance;

public class PyFinanceClient(IHttpClientFactory httpClientFactory) : IMarketDataClient
{
    readonly HttpClient _searchClient = httpClientFactory.CreateClient("PyFinanceSearch");

    readonly HttpClient _marketDataClient = httpClientFactory.CreateClient("PyFinanceMarketData");

    public async Task<IEnumerable<Domain.Entities.Securities.SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken)
    {
        var response = await _searchClient.GetAsync($"search?q={Uri.EscapeDataString(userQuery)}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var resultDtos = await response.Content.ReadFromJsonAsync<List<SearchResult>>(cancellationToken);
        return resultDtos?.Select(dto => dto.ToDomainEntity()) ?? [];
    }
}