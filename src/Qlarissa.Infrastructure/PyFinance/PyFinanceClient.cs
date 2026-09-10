using Google.Protobuf.WellKnownTypes;
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

    public Task<PubliclyTradedSecurityBase?> GetSecurityWithHistoryAsync(string tickerSymbol, CancellationToken cancellationToken)
        => GetSecurityWithHistoryFromDateAsync(tickerSymbol, _options.MarketDataAPI.StartDate, cancellationToken);

    public async Task<PubliclyTradedSecurityBase?> GetSecurityWithHistoryFromDateAsync(string tickerSymbol, DateOnly date, CancellationToken cancellationToken)
    {
        var response = await _marketDataClient.GetAsync($"security?symbol={Uri.EscapeDataString(tickerSymbol)}&startdate={date:yyyy-MM-dd}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var resultDto = await response.Content.ReadFromJsonAsync<PyFinance.Security>(cancellationToken);

        if (resultDto == null)
        {
            return null;
        }

        if (resultDto.Info.MarketState == "REGULAR") // The market is still open and the last entry not final. Do not add it to the history.
        {
            resultDto.History.RemoveAt(resultDto.History.Count - 1);
        }

        return resultDto.ToDomainEntity();
    }
}