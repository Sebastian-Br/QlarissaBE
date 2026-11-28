using Qlarissa.WebAPI.Models.Settings.Graph;

namespace Qlarissa.WebAPI.Tests;

public class SettingsTests
{
    [Fact]
    public void GraphSettingsBase_ToDomainEntity_ResolvesType()
    {
        GraphSettingsBase apiModel = new DividendPayoutGraphSettings();
        var domainEntity = apiModel.ToDomainEntity() as Domain.Settings.Graph.DividendPayoutGraphSettings;
        Assert.NotNull(domainEntity);
    }

    [Fact]
    public void GraphSettingsBase_FromDomainEntity_ResolvesType()
    {
        Domain.Settings.Graph.GraphSettingBase domainEntity = new Domain.Settings.Graph.DividendPayoutGraphSettings();
        var apiModel = GraphSettingsBase.FromDomainEntity(domainEntity) as DividendPayoutGraphSettings;
        Assert.NotNull(apiModel);
    }

    [Fact]
    public void DividendPayoutGraphSettings_ToDomainEntity()
    {
        var apiModel = new DividendPayoutGraphSettings
        {
            DataSource = Domain.Settings.Graph.GraphDataSource.DividendPayouts,
            IsSynchronizedWithSecurityGraph = false,
            Bounds = new Models.Settings.Bounds { StartDate = new DateOnly(2024, 1, 1) }
        };

        var domainEntity = apiModel.ToDomainEntity();

        Assert.Equal(apiModel.DataSource, domainEntity.DataSource);
        Assert.Equal(apiModel.IsSynchronizedWithSecurityGraph, domainEntity.IsSynchronizedWithSecurityGraph);
        Assert.Equal(apiModel.Bounds?.StartDate, domainEntity.Bounds?.StartDate);
    }

    [Fact]
    public void DividendPayoutGraphSettings_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.DividendPayoutGraphSettings
        {
            Id = 7,
            DataSource = Domain.Settings.Graph.GraphDataSource.DividendPayouts,
            IsSynchronizedWithSecurityGraph = false,
            Bounds = new Domain.Settings.Bounds { StartDate = new DateOnly(2024, 1, 1) }
        };

        var apiModel = GraphSettingsBase.FromDomainEntity(domainEntity) as DividendPayoutGraphSettings;

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.DataSource, apiModel.DataSource);
        Assert.Equal(domainEntity.IsSynchronizedWithSecurityGraph, apiModel.IsSynchronizedWithSecurityGraph);
        Assert.Equal(domainEntity.Bounds?.StartDate, apiModel.Bounds?.StartDate);
    }

    [Fact]
    public void SecurityGraphSettings_ToDomainEntity()
    {
        var apiModel = new SecurityGraphSettings
        {
            DataSource = Domain.Settings.Graph.GraphDataSource.Security,
            AsTotalReturn = true,
            Bounds = new Models.Settings.Bounds { EndDate = new DateOnly(2025, 1, 1) }
        };

        var domainEntity = apiModel.ToDomainEntity();

        Assert.Equal(apiModel.DataSource, domainEntity.DataSource);
        Assert.Equal(apiModel.AsTotalReturn, domainEntity.AsTotalReturn);
        Assert.Equal(apiModel.Bounds?.EndDate, domainEntity.Bounds?.EndDate);
    }

    [Fact]
    public void SecurityGraphSettings_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.SecurityGraphSettings
        {
            Id = 5,
            DataSource = Domain.Settings.Graph.GraphDataSource.Security,
            AsTotalReturn = true,
            Bounds = new Domain.Settings.Bounds { EndDate = new DateOnly(2025, 1, 1) }
        };

        var apiModel = GraphSettingsBase.FromDomainEntity(domainEntity) as SecurityGraphSettings;

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.DataSource, apiModel.DataSource);
        Assert.Equal(domainEntity.AsTotalReturn, apiModel.AsTotalReturn);
        Assert.Equal(domainEntity.Bounds?.EndDate, apiModel.Bounds?.EndDate);
    }
}
