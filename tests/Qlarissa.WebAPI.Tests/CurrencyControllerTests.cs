using Microsoft.AspNetCore.Mvc;
using Moq;
using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Controllers;

namespace Qlarissa.WebAPI.Tests;

public class CurrencyControllerTests
{
    [Fact]
    public void DependencyIsMissing_ShouldThrow()
    {
        var currencyManagerMock = new Mock<ICurrencyManager>();
        Assert.Throws<ArgumentNullException>(() => new CurrencyController(null!));
        Assert.NotNull(new CurrencyController(currencyManagerMock.Object));
    }

    [Fact]
    public async Task GetCurrenciesAsync_ShouldMapDomainEntitiesToModels()
    {
        var currencyManagerMock = new Mock<ICurrencyManager>();
        currencyManagerMock.Setup(mgr => mgr.GetCurrenciesAsync()).ReturnsAsync(
        [
            new Domain.Entities.Currency { Id = 1, Symbol = "USD", Name = "US Dollar" },
            new Domain.Entities.Currency { Id = 2, Symbol = "EUR", Name = "Euro" }
        ]);
        var controller = new CurrencyController(currencyManagerMock.Object);

        var result = await controller.GetCurrenciesAsync();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var currencies = Assert.IsAssignableFrom<IEnumerable<WebAPI.Models.Currency>>(okResult.Value);
        Assert.NotNull(currencies);
        Assert.Equal(2, currencies.Count());
        Assert.Contains(currencies, c => c.Id == 1 && c.Symbol == "USD" && c.Name == "US Dollar");
        Assert.Contains(currencies, c => c.Id == 2 && c.Symbol == "EUR" && c.Name == "Euro");
    }
}