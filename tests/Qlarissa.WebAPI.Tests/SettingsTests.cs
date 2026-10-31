

namespace Qlarissa.WebAPI.Tests;

public class SettingsTests
{
    [Fact]
    public void DividendPayoutGraphSettings_ToDomainEntity()
    {
        var apiModel = new Models.GraphSettings.DividendPayoutGraphSettings
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
            LogarithmicScale = true,
            PriceCardHoverDistance = 10,
            ShowDividendYield = true,
            IsSynchronizedWithSecurityGraph = false
        };

        var domainEntity = apiModel.ToDomainEntity();

        Assert.Equal(apiModel.StartDate, domainEntity.StartDate);
        Assert.Equal(apiModel.EndDate, domainEntity.EndDate);
        Assert.Equal(apiModel.LogarithmicScale, domainEntity.LogarithmicScale);
        Assert.Equal(apiModel.PriceCardHoverDistance, domainEntity.PriceCardHoverDistance);
        Assert.Equal(apiModel.ShowDividendYield, domainEntity.ShowDividendYield);
        Assert.Equal(apiModel.IsSynchronizedWithSecurityGraph, domainEntity.IsSynchronizedWithSecurityGraph);
    }

    [Fact]
    public void DividendPayoutGraphSettings_FromDomainEntity()
    {
        var domainEntity = new Domain.GraphSettings.DividendPayoutGraphSettings
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
            LogarithmicScale = true,
            PriceCardHoverDistance = 10,
            ShowDividendYield = true,
            IsSynchronizedWithSecurityGraph = false
        };
        var apiModel = new Models.GraphSettings.DividendPayoutGraphSettings();
        apiModel.FromDomainEntity(domainEntity);
        Assert.Equal(domainEntity.StartDate, apiModel.StartDate);
        Assert.Equal(domainEntity.EndDate, apiModel.EndDate);
        Assert.Equal(domainEntity.LogarithmicScale, apiModel.LogarithmicScale);
        Assert.Equal(domainEntity.PriceCardHoverDistance, apiModel.PriceCardHoverDistance);
        Assert.Equal(domainEntity.ShowDividendYield, apiModel.ShowDividendYield);
        Assert.Equal(domainEntity.IsSynchronizedWithSecurityGraph, apiModel.IsSynchronizedWithSecurityGraph);
    }

    [Fact]
    public void SecurityGraphSettings_ToDomainEntity()
    {
        var apiModel = new Models.GraphSettings.SecurityGraphSettings
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
            LogarithmicScale = true,
            PriceCardHoverDistance = 10,
        };
        var domainEntity = apiModel.ToDomainEntity();
        Assert.Equal(apiModel.StartDate, domainEntity.StartDate);
        Assert.Equal(apiModel.EndDate, domainEntity.EndDate);
        Assert.Equal(apiModel.LogarithmicScale, domainEntity.LogarithmicScale);
        Assert.Equal(apiModel.PriceCardHoverDistance, domainEntity.PriceCardHoverDistance);
    }

    [Fact]
    public void SecurityGraphSettings_FromDomainEntity()
    {
        var domainEntity = new Domain.GraphSettings.SecurityGraphSettings
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
            LogarithmicScale = true,
            PriceCardHoverDistance = 10,
        };
        var apiModel = new Models.GraphSettings.SecurityGraphSettings();
        apiModel.FromDomainEntity(domainEntity);
        Assert.Equal(domainEntity.StartDate, apiModel.StartDate);
        Assert.Equal(domainEntity.EndDate, apiModel.EndDate);
        Assert.Equal(domainEntity.LogarithmicScale, apiModel.LogarithmicScale);
        Assert.Equal(domainEntity.PriceCardHoverDistance, apiModel.PriceCardHoverDistance);
    }
}