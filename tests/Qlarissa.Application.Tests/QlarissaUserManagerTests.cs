using Moq;
using Qlarissa.Domain.Entities;
using Qlarissa.Infrastructure.Authorization;
using Qlarissa.Infrastructure.DB.Repositories.Interfaces;
using System.Security.Claims;

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

    [Fact]
    public async Task LoginAsync_GetAsyncCallFails_ShouldReturnFail()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        string testUserName = "testUser";
        string testUserPassword = "testPassword1!";
        userRepositoryMock.Setup(x => x.GetAsync(testUserName)).ReturnsAsync(null as QlarissaUser);
        userRepositoryMock.Setup(x => x.CheckPasswordAsync(It.IsAny<QlarissaUser>(), testUserPassword)).ReturnsAsync(true);
        var sut = new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.LoginAsync(testUserName, testUserPassword);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task LoginAsync_RepositoryCallFails_ShouldReturnFail()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        QlarissaUser user = new() { UserName = "testUser" };
        string testUserPassword = "testPassword1!";
        userRepositoryMock.Setup(x => x.GetAsync(user.UserName)).ReturnsAsync(user);
        userRepositoryMock.Setup(x => x.CheckPasswordAsync(user, testUserPassword)).ReturnsAsync(false);
        var sut = new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.LoginAsync(user.UserName, testUserPassword);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task LoginAsync_RepositoryCallsSucceed_ShouldReturnOkWithJwt()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        QlarissaUser user = new() { UserName = "testUser" };
        string testUserPassword = "testPassword1!";
        userRepositoryMock.Setup(x => x.GetAsync(user.UserName)).ReturnsAsync(user);
        userRepositoryMock.Setup(x => x.CheckPasswordAsync(user, testUserPassword)).ReturnsAsync(true);
        string token = "SecureToken";
        jwtServiceMock.Setup(x => x.GenerateToken(user)).Returns(token);
        var sut = new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.LoginAsync(user.UserName, testUserPassword);
        Assert.True(result.IsSuccess);
        Assert.Equal(token, result.Value);
    }

    [Fact]
    public async Task GetAsync_NullClaim_ShouldReturnFail()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        string userName = "testUser";
        QlarissaUser user = new() { UserName = userName };
        userRepositoryMock.Setup(x => x.GetAsync(userName)).ReturnsAsync(user);
        var sut = new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.GetAsync(null!);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task GetAsync_RepositoryCallFails_ShouldReturnFail()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        string userName = "testUser";
        var claims = new List<Claim>() { new(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);
        userRepositoryMock.Setup(x => x.GetAsync(userName)).ReturnsAsync(null as QlarissaUser);
        var sut = new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.GetAsync(principal);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task GetAsync_RepositoryCallSucceeds_ShouldReturnOkWithUser()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        string userName = "testUser";
        var claims = new List<Claim>() { new(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);
        QlarissaUser user = new() { UserName = userName };
        userRepositoryMock.Setup(x => x.GetAsync(userName)).ReturnsAsync(user);
        var sut = new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.GetAsync(principal);
        Assert.True(result.IsSuccess);
        Assert.Equal(user, result.Value);
    }
}