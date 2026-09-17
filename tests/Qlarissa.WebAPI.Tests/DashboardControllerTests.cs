using Moq;
using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Controllers;

namespace Qlarissa.WebAPI.Tests;

public class DashboardControllerTests
{
    [Fact]
    public void DependencyIsMissing_ShouldThrow()
    {
        var securityManagerMock = new Mock<ISecurityManager>();
        var qlarissaUserManagerMock = new Mock<IQlarissaUserManager>();
        Assert.Throws<ArgumentNullException>(() => new DashboardController(null!, qlarissaUserManagerMock.Object));
        Assert.Throws<ArgumentNullException>(() => new DashboardController(securityManagerMock.Object, null!));
        Assert.NotNull(new DashboardController(securityManagerMock.Object, qlarissaUserManagerMock.Object));
    }
}