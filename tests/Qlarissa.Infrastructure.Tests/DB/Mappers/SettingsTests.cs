
using Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

namespace Qlarissa.Infrastructure.Tests.DB.Mappers;

public sealed class SettingsTests
{
    [Fact]
    public void GraphSettingsBase_ToDomainEntity_ResolvesType()
    {
        GraphSettingsBase dbEntity = new DividendPayoutGraphSettings();
        var domainEntity = dbEntity.ToDomainEntity() as Domain.Settings.Graph.DividendPayoutGraphSettings;
        Assert.NotNull(domainEntity);
    }

    [Fact]
    public void GraphSettingsBase_FromDomainEntity_ResolvesType()
    {
        Domain.Settings.Graph.GraphSettingBase domainEntity = new Domain.Settings.Graph.DividendPayoutGraphSettings();
        var dbEntity = GraphSettingsBase.FromDomainEntity(domainEntity, "") as DividendPayoutGraphSettings;
        Assert.NotNull(dbEntity);
    }

    [Fact]
    public void SecurityGraph_ToDomainEntity()
    {
        var dbEntity = new SecurityGraphSettings
        {
            Id = 5,
            UserId = "user-123",
            GraphType = GraphType.SecurityGraph,
            Bounds = new Infrastructure.DB.Entities.Settings.Bounds
            {
                Id = 11,
                StartDate = new DateOnly(2023, 1, 1),
                EndDate = new DateOnly(2023, 12, 31),
                Start = 25.5,
                End = 99.9
            },
            TickCount = 7,
            ShowXAxisGridLines = true,
            ShowYAxisGridLines = false,
            LogarithmicScale = true,
            PriceCardHoverDistance = 14
        };

        var domainEntity = dbEntity.ToDomainEntity() as Domain.Settings.Graph.SecurityGraphSettings;

        Assert.NotNull(domainEntity);
        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal((Domain.Settings.Graph.GraphType)dbEntity.GraphType, domainEntity.GraphType);
        Assert.NotNull(domainEntity.Bounds);
        Assert.Equal(dbEntity.Bounds.StartDate, domainEntity.Bounds.StartDate);
        Assert.Equal(dbEntity.Bounds.EndDate, domainEntity.Bounds.EndDate);
        Assert.Equal(dbEntity.Bounds.Start, domainEntity.Bounds.Start);
        Assert.Equal(dbEntity.Bounds.End, domainEntity.Bounds.End);
        Assert.Equal(dbEntity.TickCount, domainEntity.TickCount);
        Assert.Equal(dbEntity.ShowXAxisGridLines, domainEntity.ShowXAxisGridLines);
        Assert.Equal(dbEntity.ShowYAxisGridLines, domainEntity.ShowYAxisGridLines);
        Assert.Equal(dbEntity.LogarithmicScale, domainEntity.LogarithmicScale);
        Assert.Equal(dbEntity.PriceCardHoverDistance, domainEntity.ValueCardHoverDistance);
    }

    [Fact]
    public void SecurityGraph_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.SecurityGraphSettings
        {
            Id = 6,
            GraphType = Domain.Settings.Graph.GraphType.Security,
            Bounds = new Domain.Settings.Bounds
            {
                Id = 12,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 12, 31),
                Start = 30.0,
                End = 150.0
            },
            TickCount = 9,
            ShowXAxisGridLines = true,
            ShowYAxisGridLines = true,
            LogarithmicScale = false,
            ValueCardHoverDistance = 22
        };

        var dbEntity = GraphSettingsBase.FromDomainEntity(domainEntity, "user-456") as SecurityGraphSettings;

        Assert.NotNull(dbEntity);
        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal("user-456", dbEntity.UserId);
        Assert.Equal((GraphType)domainEntity.GraphType, dbEntity.GraphType);
        Assert.NotNull(dbEntity.Bounds);
        Assert.Equal(domainEntity.Bounds.StartDate, dbEntity.Bounds.StartDate);
        Assert.Equal(domainEntity.Bounds.EndDate, dbEntity.Bounds.EndDate);
        Assert.Equal(domainEntity.Bounds.Start, dbEntity.Bounds.Start);
        Assert.Equal(domainEntity.Bounds.End, dbEntity.Bounds.End);
        Assert.Equal(domainEntity.TickCount, dbEntity.TickCount);
        Assert.Equal(domainEntity.ShowXAxisGridLines, dbEntity.ShowXAxisGridLines);
        Assert.Equal(domainEntity.ShowYAxisGridLines, dbEntity.ShowYAxisGridLines);
        Assert.Equal(domainEntity.LogarithmicScale, dbEntity.LogarithmicScale);
        Assert.Equal(domainEntity.ValueCardHoverDistance, dbEntity.PriceCardHoverDistance);
    }

    [Fact]
    public void DividendPayoutGraph_ToDomainEntity()
    {
        var dbEntity = new DividendPayoutGraphSettings
        {
            Id = 7,
            UserId = "user-789",
            GraphType = GraphType.DividendPayoutGraph,
            Bounds = new Infrastructure.DB.Entities.Settings.Bounds
            {
                Id = 13,
                StartDate = new DateOnly(2022, 1, 1),
                EndDate = new DateOnly(2022, 12, 31),
                Start = 1.25,
                End = 5.5
            },
            TickCount = 6,
            ShowXAxisGridLines = false,
            ShowYAxisGridLines = true,
            LogarithmicScale = true,
            PriceCardHoverDistance = 18,
            ShowDividendYield = true,
            IsSynchronizedWithSecurityGraph = false
        };

        var domainEntity = dbEntity.ToDomainEntity();

        Assert.NotNull(domainEntity);
        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal((Domain.Settings.Graph.GraphType)dbEntity.GraphType, domainEntity.GraphType);
        Assert.NotNull(domainEntity.Bounds);
        Assert.Equal(dbEntity.Bounds.StartDate, domainEntity.Bounds.StartDate);
        Assert.Equal(dbEntity.Bounds.EndDate, domainEntity.Bounds.EndDate);
        Assert.Equal(dbEntity.Bounds.Start, domainEntity.Bounds.Start);
        Assert.Equal(dbEntity.Bounds.End, domainEntity.Bounds.End);
        Assert.Equal(dbEntity.TickCount, domainEntity.TickCount);
        Assert.Equal(dbEntity.ShowXAxisGridLines, domainEntity.ShowXAxisGridLines);
        Assert.Equal(dbEntity.ShowYAxisGridLines, domainEntity.ShowYAxisGridLines);
        Assert.Equal(dbEntity.LogarithmicScale, domainEntity.LogarithmicScale);
        Assert.Equal(dbEntity.PriceCardHoverDistance, domainEntity.ValueCardHoverDistance);
        Assert.Equal(dbEntity.ShowDividendYield, domainEntity.ShowDividendYield);
        Assert.Equal(dbEntity.IsSynchronizedWithSecurityGraph, domainEntity.IsSynchronizedWithSecurityGraph);
    }

    [Fact]
    public void DividendPayoutGraph_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.DividendPayoutGraphSettings
        {
            Id = 8,
            GraphType = Domain.Settings.Graph.GraphType.DividendPayouts,
            Bounds = new Domain.Settings.Bounds
            {
                Id = 14,
                StartDate = new DateOnly(2025, 1, 1),
                EndDate = new DateOnly(2025, 12, 31),
                Start = 2.0,
                End = 10.0
            },
            TickCount = 8,
            ShowXAxisGridLines = false,
            ShowYAxisGridLines = true,
            LogarithmicScale = false,
            ValueCardHoverDistance = 19,
            ShowDividendYield = true,
            IsSynchronizedWithSecurityGraph = true
        };

        var dbEntity = GraphSettingsBase.FromDomainEntity(domainEntity, "user-101") as DividendPayoutGraphSettings;

        Assert.NotNull(dbEntity);
        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal("user-101", dbEntity.UserId);
        Assert.Equal((GraphType)domainEntity.GraphType, dbEntity.GraphType);
        Assert.NotNull(dbEntity.Bounds);
        Assert.Equal(domainEntity.Bounds.StartDate, dbEntity.Bounds.StartDate);
        Assert.Equal(domainEntity.Bounds.EndDate, dbEntity.Bounds.EndDate);
        Assert.Equal(domainEntity.Bounds.Start, dbEntity.Bounds.Start);
        Assert.Equal(domainEntity.Bounds.End, dbEntity.Bounds.End);
        Assert.Equal(domainEntity.TickCount, dbEntity.TickCount);
        Assert.Equal(domainEntity.ShowXAxisGridLines, dbEntity.ShowXAxisGridLines);
        Assert.Equal(domainEntity.ShowYAxisGridLines, dbEntity.ShowYAxisGridLines);
        Assert.Equal(domainEntity.LogarithmicScale, dbEntity.LogarithmicScale);
        Assert.Equal(domainEntity.ValueCardHoverDistance, dbEntity.PriceCardHoverDistance);
        Assert.Equal(domainEntity.ShowDividendYield, dbEntity.ShowDividendYield);
        Assert.Equal(domainEntity.IsSynchronizedWithSecurityGraph, dbEntity.IsSynchronizedWithSecurityGraph);
    }
}
