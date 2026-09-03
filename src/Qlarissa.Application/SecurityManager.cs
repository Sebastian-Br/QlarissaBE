using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.ExternalAPI;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application;

public sealed class SecurityManager(ISecurityRepository securityRepository, ICurrencyRepository currencyRepository, IMarketDataClient marketDataClient) : ISecurityManager
{
    readonly ISecurityRepository _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));

    readonly ICurrencyRepository _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));

    readonly IMarketDataClient _marketDataClient = marketDataClient ?? throw new ArgumentNullException(nameof(marketDataClient));

    public Task<PubliclyTradedSecurityBase?> GetSecurityAsync(int id, CancellationToken cancellationToken)
        => _securityRepository.GetSecurityAsync(id, cancellationToken);

    public async Task<FluentResults.Result<string>> AddSecurityAsync(string securityTickerSymbol, CancellationToken cancellationToken)
    {
        if (await _securityRepository.SecurityExistsAsync(securityTickerSymbol))
        {
            return FluentResults.Result.Fail($"Security with ticker symbol '{securityTickerSymbol}' already exists.");
        }

        var domainEntity = await _marketDataClient.GetSecurityAsync(securityTickerSymbol, cancellationToken);
        var currency = await _currencyRepository.GetCurrencyAsync(domainEntity.Currency.Symbol);

        if (currency == null)
        {
            return FluentResults.Result.Fail($"Currency with symbol '{domainEntity.Currency.Symbol}' does not exist.");
        }

        domainEntity.Currency = currency;
        await _securityRepository.AddSecurityAsync(domainEntity, cancellationToken);

        return FluentResults.Result.Ok(domainEntity.Symbol);
    }

    public Task<bool> SecurityExistsAsync(string securityTickerSymbol)
        => _securityRepository.SecurityExistsAsync(securityTickerSymbol);

    public Task<IEnumerable<SearchResult>> SearchSecuritiesExternallyAsync(string userQuery, CancellationToken cancellationToken)
        => _marketDataClient.SearchSecuritiesAsync(userQuery, cancellationToken);

    public Task<IEnumerable<SearchResult>> SearchSecuritiesInternallyAsync(string userQuery, CancellationToken cancellationToken)
        => _securityRepository.SearchSecuritiesAsync(userQuery, cancellationToken);
}