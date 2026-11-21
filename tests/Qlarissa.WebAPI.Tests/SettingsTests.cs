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
            DataSource = GraphDataSource.DividendPayouts,
            Bounds = new()
            {
                StartDate = new DateOnly(2023, 1, 1),
                EndDate = new DateOnly(2023, 12, 31),
                Start = 2023.0,
                End = 2023.99
            },
            TickCount = 5,
            ShowHorizontalGridLines = true,
            ShowVerticalGridLines = true,
            LogarithmicScale = true,
            ValueCardHoverDistance = 10,
            ShowDividendYield = true,
            IsSynchronizedWithSecurityGraph = false
        };

        var domainEntity = apiModel.ToDomainEntity();

        Assert.Equal(apiModel.DataSource, (GraphDataSource)domainEntity.DataSource);
        Assert.NotNull(domainEntity.Bounds);
        Assert.Equal(apiModel.Bounds.StartDate, domainEntity.Bounds.StartDate);
        Assert.Equal(apiModel.Bounds.EndDate, domainEntity.Bounds.EndDate);
        Assert.Equal(apiModel.Bounds.Start, domainEntity.Bounds.Start);
        Assert.Equal(apiModel.Bounds.End, domainEntity.Bounds.End);
        Assert.Equal(apiModel.TickCount, domainEntity.TickCount);
        Assert.Equal(apiModel.ShowVerticalGridLines, domainEntity.ShowVerticalGridLines);
        Assert.Equal(apiModel.ShowVerticalGridLines, domainEntity.ShowVerticalGridLines);
        Assert.Equal(apiModel.LogarithmicScale, domainEntity.LogarithmicScale);
        Assert.Equal(apiModel.ValueCardHoverDistance, domainEntity.ValueCardHoverDistance);
        Assert.Equal(apiModel.ShowDividendYield, domainEntity.ShowDividendYield);
        Assert.Equal(apiModel.IsSynchronizedWithSecurityGraph, domainEntity.IsSynchronizedWithSecurityGraph);
    }

    [Fact]
    public void DividendPayoutGraphSettings_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.DividendPayoutGraphSettings
        {
            Id = 7,
            DataSource = Domain.Settings.Graph.GraphDataSource.DividendPayouts,
            Bounds = new()
            {
                StartDate = new DateOnly(2023, 1, 1),
                EndDate = new DateOnly(2023, 12, 31),
                Start = 2023.0,
                End = 2023.99
            },
            TickCount = 5,
            ShowHorizontalGridLines = true,
            ShowVerticalGridLines = true,
            LogarithmicScale = true,
            ValueCardHoverDistance = 10,
            ShowDividendYield = true,
            IsSynchronizedWithSecurityGraph = false
        };

        var apiModel = GraphSettingsBase.FromDomainEntity(domainEntity) as DividendPayoutGraphSettings;

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.DataSource, (Domain.Settings.Graph.GraphDataSource)apiModel.DataSource);
        Assert.NotNull(apiModel.Bounds);
        Assert.Equal(domainEntity.Bounds.StartDate, apiModel.Bounds.StartDate);
        Assert.Equal(domainEntity.Bounds.EndDate, apiModel.Bounds.EndDate);
        Assert.Equal(domainEntity.Bounds.Start, apiModel.Bounds.Start);
        Assert.Equal(domainEntity.Bounds.End, apiModel.Bounds.End);
        Assert.Equal(domainEntity.TickCount, apiModel.TickCount);
        Assert.Equal(domainEntity.ShowHorizontalGridLines, apiModel.ShowHorizontalGridLines);
        Assert.Equal(domainEntity.ShowVerticalGridLines, apiModel.ShowVerticalGridLines);
        Assert.Equal(domainEntity.TickCount, apiModel.TickCount);
        Assert.Equal(domainEntity.LogarithmicScale, apiModel.LogarithmicScale);
        Assert.Equal(domainEntity.ValueCardHoverDistance, apiModel.ValueCardHoverDistance);
        Assert.Equal(domainEntity.ShowDividendYield, apiModel.ShowDividendYield);
        Assert.Equal(domainEntity.IsSynchronizedWithSecurityGraph, apiModel.IsSynchronizedWithSecurityGraph);
    }

    [Fact]
    public void SecurityGraphSettings_ToDomainEntity()
    {
        var apiModel = new  SecurityGraphSettings
        {
            DataSource = GraphDataSource.Security,
            TickCount = 5,
            ShowHorizontalGridLines = true,
            ShowVerticalGridLines = true,
            LogarithmicScale = true,
            ValueCardHoverDistance = 10,
        };

        var domainEntity = apiModel.ToDomainEntity();

        Assert.Equal(apiModel.DataSource, (GraphDataSource)domainEntity.DataSource);
        Assert.Null(domainEntity.Bounds);
        Assert.Equal(apiModel.TickCount, domainEntity.TickCount);
        Assert.Equal(apiModel.ShowHorizontalGridLines, domainEntity.ShowHorizontalGridLines);
        Assert.Equal(apiModel.ShowVerticalGridLines, domainEntity.ShowVerticalGridLines);
        Assert.Equal(apiModel.LogarithmicScale, domainEntity.LogarithmicScale);
        Assert.Equal(apiModel.ValueCardHoverDistance, domainEntity.ValueCardHoverDistance);
    }

    [Fact]
    public void SecurityGraphSettings_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.SecurityGraphSettings
        {
            Id = 5,
            DataSource = Domain.Settings.Graph.GraphDataSource.Security,
            TickCount = 5,
            ShowHorizontalGridLines = true,
            ShowVerticalGridLines = true,
            LogarithmicScale = true,
            ValueCardHoverDistance = 10,
        };

        var apiModel = GraphSettingsBase.FromDomainEntity(domainEntity) as SecurityGraphSettings;

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.DataSource, (Domain.Settings.Graph.GraphDataSource)apiModel.DataSource);
        Assert.Null(apiModel.Bounds);
        Assert.Equal(domainEntity.TickCount, apiModel.TickCount);
        Assert.Equal(domainEntity.ShowHorizontalGridLines, apiModel.ShowHorizontalGridLines);
        Assert.Equal(domainEntity.ShowVerticalGridLines, apiModel.ShowVerticalGridLines);
        Assert.Equal(domainEntity.LogarithmicScale, apiModel.LogarithmicScale);
        Assert.Equal(domainEntity.ValueCardHoverDistance, apiModel.ValueCardHoverDistance);
    }
}