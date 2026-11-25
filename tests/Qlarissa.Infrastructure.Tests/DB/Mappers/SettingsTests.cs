
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
            UserId = "user-123",
            DataSource = GraphDataSource.Security,
            AsTotalReturn = true
        };

        var domainEntity = dbEntity.ToDomainEntity() as Domain.Settings.Graph.SecurityGraphSettings;

        Assert.NotNull(domainEntity);
        Assert.Equal((Domain.Settings.Graph.GraphDataSource)dbEntity.DataSource, domainEntity.DataSource);
        Assert.Equal(dbEntity.AsTotalReturn, domainEntity.AsTotalReturn);
    }

    [Fact]
    public void SecurityGraph_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.SecurityGraphSettings
        {
            Id = 6,
            DataSource = Domain.Settings.Graph.GraphDataSource.Security,
            AsTotalReturn = true
        };

        var dbEntity = GraphSettingsBase.FromDomainEntity(domainEntity, "user-456") as SecurityGraphSettings;

        Assert.NotNull(dbEntity);
        Assert.Equal("user-456", dbEntity.UserId);
        Assert.Equal((GraphDataSource)domainEntity.DataSource, dbEntity.DataSource);
        Assert.Equal(domainEntity.AsTotalReturn, dbEntity.AsTotalReturn);
    }

    [Fact]
    public void DividendPayoutGraph_ToDomainEntity()
    {
        var dbEntity = new DividendPayoutGraphSettings
        {
            UserId = "user-789",
            DataSource = GraphDataSource.DividendPayouts,
            IsSynchronizedWithSecurityGraph = false
        };

        var domainEntity = dbEntity.ToDomainEntity();

        Assert.NotNull(domainEntity);
        Assert.Equal((Domain.Settings.Graph.GraphDataSource)dbEntity.DataSource, domainEntity.DataSource);
        Assert.Equal(dbEntity.IsSynchronizedWithSecurityGraph, domainEntity.IsSynchronizedWithSecurityGraph);
    }

    [Fact]
    public void DividendPayoutGraph_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.DividendPayoutGraphSettings
        {
            Id = 8,
            DataSource = Domain.Settings.Graph.GraphDataSource.DividendPayouts,
            IsSynchronizedWithSecurityGraph = true
        };

        var dbEntity = GraphSettingsBase.FromDomainEntity(domainEntity, "user-101") as DividendPayoutGraphSettings;

        Assert.NotNull(dbEntity);
        Assert.Equal("user-101", dbEntity.UserId);
        Assert.Equal((GraphDataSource)domainEntity.DataSource, dbEntity.DataSource);
        Assert.Equal(domainEntity.IsSynchronizedWithSecurityGraph, dbEntity.IsSynchronizedWithSecurityGraph);
    }
}
