using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;
using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DashboardController(ISecurityManager qlarissaSecurityManager) : ControllerBase
{
    readonly ISecurityManager _qlarissaSecurityManager = qlarissaSecurityManager;

    [HttpGet]
    public async Task<IActionResult> SeedAsync()
    {
        var priceHistoryListCurrency = new List<DailyPrice>
        {
            new() { Average = 1, Close = 1, Open = 1, Low = 1, High = 1, Date = new(2022, 12, 1) },
            new() { Average = 1, Close = 1, Open = 1, Low = 1, High = 1, Date = new(2022, 12, 2) },
            new() { Average = 1, Close = 1, Open = 1, Low = 1, High = 1, Date = new(2022, 12, 3) }
        };
        var currency = new Currency() { Currency = null, Name = "US Dollar", Symbol = "USD", Price = 1, PriceHistoryForRegression = null, PriceHistory = [.. priceHistoryListCurrency] };
        var priceHistoryList = new List<DailyPrice>
        {
            new() { Average = 400, Close = 392, Open = 398, Low = 390, High = 410, Date = new(2022, 12, 1) },
            new() { Average = 402, Close = 393, Open = 399, Low = 392, High = 412, Date = new(2022, 12, 2) },
            new() { Average = 400, Close = 395, Open = 404, Low = 395, High = 405, Date = new(2022, 12, 3) },
            new() { Average = 405, Close = 403, Open = 403, Low = 400, High = 410, Date = new(2022, 12, 4) }
        };
        var security = new Stock() { Name = "Microsoft Corporation", Symbol = "MSFT", Currency = currency, PriceHistory = [.. priceHistoryList], PriceHistoryForRegression = null, InvestorRelationsURL = "https://investors.microsoft.com", Price = 405 };
        await _qlarissaSecurityManager.AddSecurityAsync(security);

        return Ok();
    }
}