using Moq;
using Qlarissa.Application;
using Qlarissa.Application.Repositories;
using Qlarissa.Application.Settings;
using Qlarissa.Domain.Settings;

namespace Qlarissa.Application.Tests;

public sealed class DisplaySettingsTests
{
    [Fact]
    public async Task SyncDisplaySettings_AllowsNamespacedKeyWithoutSemanticValidation()
    {
        var repository = new Mock<ISettingsRepository>();
        repository.Setup(repo => repo.SyncDisplaySettingsAsync(
                It.IsAny<string>(),
                It.IsAny<DeviceProfile>(),
                It.IsAny<IReadOnlyCollection<DisplaySettingChange>>(),
                It.IsAny<int>()))
            .ReturnsAsync(FluentResults.Result.Ok());
        var manager = new SettingsManager(repository.Object, new DisplaySettingsOptions());

        var result = await manager.SyncDisplaySettingsAsync(
            "user",
            DeviceProfile.Desktop,
            [new DisplaySettingChange("custom.key", "anything")]);

        Assert.True(result.IsSuccess);
        repository.Verify(
            repo => repo.SyncDisplaySettingsAsync(
                It.IsAny<string>(),
                It.IsAny<DeviceProfile>(),
                It.IsAny<IReadOnlyCollection<DisplaySettingChange>>(),
                It.IsAny<int>()),
            Times.Once);
    }

    [Fact]
    public async Task SyncDisplaySettings_RejectsValuesOverCharacterLimit()
    {
        var repository = new Mock<ISettingsRepository>();
        var manager = new SettingsManager(repository.Object, new DisplaySettingsOptions { MaxValueLength = 2 });

        var result = await manager.SyncDisplaySettingsAsync(
            "user",
            DeviceProfile.Desktop,
            [new DisplaySettingChange("chart.security.hover.behavior", "ééx")]);

        Assert.True(result.IsFailed);
        repository.Verify(
            repo => repo.SyncDisplaySettingsAsync(
                It.IsAny<string>(),
                It.IsAny<DeviceProfile>(),
                It.IsAny<IReadOnlyCollection<DisplaySettingChange>>(),
                It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task SyncDisplaySettings_AllowsValuesAtCharacterLimit()
    {
        var repository = new Mock<ISettingsRepository>();
        repository.Setup(repo => repo.SyncDisplaySettingsAsync(
                It.IsAny<string>(),
                It.IsAny<DeviceProfile>(),
                It.IsAny<IReadOnlyCollection<DisplaySettingChange>>(),
                It.IsAny<int>()))
            .ReturnsAsync(FluentResults.Result.Ok());
        var manager = new SettingsManager(repository.Object, new DisplaySettingsOptions());

        var result = await manager.SyncDisplaySettingsAsync(
            "user",
            DeviceProfile.Desktop,
            [new DisplaySettingChange("chart.security.hover.behavior", new string('x', 4000))]);

        Assert.True(result.IsSuccess);
        repository.Verify(
            repo => repo.SyncDisplaySettingsAsync(
                It.IsAny<string>(),
                It.IsAny<DeviceProfile>(),
                It.IsAny<IReadOnlyCollection<DisplaySettingChange>>(),
                It.IsAny<int>()),
            Times.Once);
    }
}
