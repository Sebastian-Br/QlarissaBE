using Qlarissa.Application.Interfaces;
using Qlarissa.Domain.Entities.Securities.Base;
using Qlarissa.Infrastructure.DB.Entities;
using Qlarissa.Infrastructure.DB.Repositories.Interfaces;

namespace Qlarissa.Application;

public sealed class SecurityManager(ISecurityRepository securityRepository) : ISecurityManager
{
    readonly ISecurityRepository _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));

    public async Task AddSecurityAsync(PubliclyTradedSecurityBase security)
    {
        if (security is Domain.Entities.Securities.Stock)
        {
            var addedCurrency = await _securityRepository.GetCurrencyAsync(security.Currency.Symbol);

            var dbEntity = new Stock
            {
                //Currency = addedCurrency,
                CurrencyId = addedCurrency.Id,
                InvestorRelationsURL = (security as Domain.Entities.Securities.Stock).InvestorRelationsURL,
                Name = security.Name,
                Symbol = security.Symbol,
                Price = security.Price
            };

            var priceHistorySecurity = new List<DailyPrice>();

            foreach (Domain.Entities.Securities.MarketData.DailyPrice dailyPrice in security.PriceHistory)
            {
                priceHistorySecurity.Add(new DailyPrice() { Average = dailyPrice.Average, Security = dbEntity, Date = dailyPrice.Date, Close = dailyPrice.Close, High = dailyPrice.High, Open = dailyPrice.Open, Low = dailyPrice.Low});
            }

            dbEntity.PriceHistory = priceHistorySecurity;

            await _securityRepository.AddSecurityAsync(dbEntity);
        }
    }
}