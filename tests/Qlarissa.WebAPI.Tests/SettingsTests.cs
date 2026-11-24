using Qlarissa.WebAPI.Models.Settings;
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
            IsSynchronizedWithSecurityGraph = false
        };

        var domainEntity = apiModel.ToDomainEntity();

        Assert.Equal(apiModel.DataSource, (GraphDataSource)domainEntity.DataSource);
        Assert.Equal(apiModel.IsSynchronizedWithSecurityGraph, domainEntity.IsSynchronizedWithSecurityGraph);
    }

    [Fact]
    public void DividendPayoutGraphSettings_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.DividendPayoutGraphSettings
        {
            Id = 7,
            DataSource = Domain.Settings.Graph.GraphDataSource.DividendPayouts,
            IsSynchronizedWithSecurityGraph = false
        };

        var apiModel = GraphSettingsBase.FromDomainEntity(domainEntity) as DividendPayoutGraphSettings;

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.DataSource, (Domain.Settings.Graph.GraphDataSource)apiModel.DataSource);
        Assert.Equal(domainEntity.IsSynchronizedWithSecurityGraph, apiModel.IsSynchronizedWithSecurityGraph);
    }

    [Fact]
    public void SecurityGraphSettings_ToDomainEntity()
    {
        var apiModel = new  SecurityGraphSettings
        {
            DataSource = GraphDataSource.Security,
        };

        var domainEntity = apiModel.ToDomainEntity();

        Assert.Equal(apiModel.DataSource, (GraphDataSource)domainEntity.DataSource);
    }

    [Fact]
    public void SecurityGraphSettings_FromDomainEntity()
    {
        var domainEntity = new Domain.Settings.Graph.SecurityGraphSettings
        {
            Id = 5,
            DataSource = Domain.Settings.Graph.GraphDataSource.Security,
        };

        var apiModel = GraphSettingsBase.FromDomainEntity(domainEntity) as SecurityGraphSettings;

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.DataSource, (Domain.Settings.Graph.GraphDataSource)apiModel.DataSource);
    }

    [Fact]
    public void SettingsTags_ToDomainEntity()
    {
        IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
        {
            ["Region"] = "Europe"
        };
        var apiModel = new SettingsTags<DividendPayoutGraphSettings>(42, tags);

        var domainEntity = apiModel.ToDomainEntity<Domain.Settings.Graph.DividendPayoutGraphSettings>();

        Assert.Equal(apiModel.SettingsTypeId, domainEntity.SettingsTypeId);
        Assert.Same(apiModel.Tags, domainEntity.Tags);
    }

    [Fact]
    public void SettingsTags_FromDomainEntity()
    {
        IReadOnlyDictionary<string, string> tags = new Dictionary<string, string>
        {
            ["Region"] = "Europe"
        };
        var domainEntity = new Domain.Settings.SettingsTags<Domain.Settings.Graph.DividendPayoutGraphSettings>(42, tags);

        var apiModel = SettingsTags<DividendPayoutGraphSettings>.FromDomainEntity(domainEntity);

        Assert.Equal(domainEntity.SettingsTypeId, apiModel.SettingsTypeId);
        Assert.Same(domainEntity.Tags, apiModel.Tags);
    }
}
