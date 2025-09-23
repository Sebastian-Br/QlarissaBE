using Moq;
using Qlarissa.Domain.Entities;
using Qlarissa.Infrastructure.Authorization;
using Qlarissa.Infrastructure.DB.Repositories.Interfaces;

namespace Qlarissa.Application.Tests;

public class QlarissaUserManagerTests
{
    [Fact]
    public void MissingDependencies_ShouldThrow()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        Assert.Throws<ArgumentNullException>(() => new QlarissaUserManager(null!, jwtServiceMock.Object));
        Assert.Throws<ArgumentNullException>(() => new QlarissaUserManager(userRepositoryMock.Object, null!));
        Assert.NotNull(new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object));
    }

    [Fact]
    public async Task RegisterAsync_RepositoryCallFails_ShouldReturnFail()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        string testUserName = "testUser";
        string testUserEmail = "test@test.com";
        string testUserPassword = "testPassword1!";
        userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<QlarissaUser>(), "testPassword1!")).ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Failed());
        var sut = new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.RegisterAsync(testUserName, testUserEmail, testUserPassword);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task RegisterAsync_RepositoryCallSucceeds_ShouldReturnOk()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        string testUserName = "testUser";
        string testUserEmail = "test@test.com";
        string testUserPassword = "testPassword1!";
        userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<QlarissaUser>(), "testPassword1!")).ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
        var sut = new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.RegisterAsync(testUserName, testUserEmail, testUserPassword);
        Assert.True(result.IsSuccess);
    }
}