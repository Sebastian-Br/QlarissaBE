using Microsoft.Extensions.Options;
using Qlarissa.Application.Interfaces.ExternalAPI;
using Qlarissa.Domain.Entities.Securities.Base;
using Qlarissa.Infrastructure.PyFinance.Options;
using System.Net.Http.Json;

namespace Qlarissa.Infrastructure.PyFinance;

public class PyFinanceClient(IHttpClientFactory httpClientFactory, IOptions<PyFinanceOptions> options) : IMarketDataClient
{
    readonly HttpClient _searchClient = httpClientFactory.CreateClient("PyFinanceSearch");

    readonly HttpClient _marketDataClient = httpClientFactory.CreateClient("PyFinanceMarketData");

    private readonly PyFinanceOptions _options = options.Value;

    public async Task<IEnumerable<Domain.Entities.Securities.SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken)
    {
        var response = await _searchClient.GetAsync($"search?q={Uri.EscapeDataString(userQuery)}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var resultDtos = await response.Content.ReadFromJsonAsync<List<SearchResult>>(cancellationToken);
        return resultDtos?.Select(dto => dto.ToDomainEntity()) ?? [];
    }

    public async Task<PubliclyTradedSecurityBase> GetSecurityAsync(string tickerSymbol, CancellationToken cancellationToken)
    {
        var response = await _marketDataClient.GetAsync($"security?symbol={Uri.EscapeDataString(tickerSymbol)}&startdate={_options.MarketDataAPI.StartDate:yyyy-MM-dd}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var resultDto = await response.Content.ReadFromJsonAsync<PyFinance.Security>(cancellationToken);
        return resultDto?.ToDomainEntity();
    }
}