using Moq;
using Qlarissa.Application.Interfaces.Authorization;
using System.Security.Claims;
using Qlarissa.Domain;
using Qlarissa.Application.Repositories;
using Qlarissa.Application.Settings;
using Qlarissa.Application.Models;

namespace Qlarissa.Application.Tests;

public class QlarissaUserManagerTests
{
    private static QlarissaUserManager CreateManager(
        IUserRepository userRepository,
        IJwtService jwtService,
        IRefreshTokenRepository? refreshTokenRepository = null) =>
        new(userRepository, jwtService, refreshTokenRepository ?? Mock.Of<IRefreshTokenRepository>(), new AuthTokenOptions());

    [Fact]
    public void MissingDependencies_ShouldThrow()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        var refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        Assert.Throws<ArgumentNullException>(() => new QlarissaUserManager(null!, jwtServiceMock.Object, refreshTokenRepositoryMock.Object, new AuthTokenOptions()));
        Assert.Throws<ArgumentNullException>(() => new QlarissaUserManager(userRepositoryMock.Object, null!, refreshTokenRepositoryMock.Object, new AuthTokenOptions()));
        Assert.Throws<ArgumentNullException>(() => new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object, null!, new AuthTokenOptions()));
        Assert.Throws<ArgumentNullException>(() => new QlarissaUserManager(userRepositoryMock.Object, jwtServiceMock.Object, refreshTokenRepositoryMock.Object, null!));
        Assert.NotNull(CreateManager(userRepositoryMock.Object, jwtServiceMock.Object, refreshTokenRepositoryMock.Object));
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
        var sut = CreateManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.RegisterAsync(testUserName, testUserEmail, 1, testUserPassword);
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
        var sut = CreateManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.RegisterAsync(testUserName, testUserEmail, 1, testUserPassword);
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
        var sut = CreateManager(userRepositoryMock.Object, jwtServiceMock.Object);
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
        var sut = CreateManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.LoginAsync(user.UserName, testUserPassword);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task LoginAsync_RepositoryCallsSucceed_ShouldReturnOkWithJwt()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        QlarissaUser user = new() { Id = "stable-user-id", UserName = "testUser" };
        string testUserPassword = "testPassword1!";
        userRepositoryMock.Setup(x => x.GetAsync(user.UserName)).ReturnsAsync(user);
        userRepositoryMock.Setup(x => x.CheckPasswordAsync(user, testUserPassword)).ReturnsAsync(true);
        string token = "SecureToken";
        var refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        refreshTokenRepositoryMock
            .Setup(repository => repository.AddAsync(
                user.Id,
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns(Task.CompletedTask);
        jwtServiceMock.Setup(x => x.GenerateToken(user)).Returns(token);
        var sut = CreateManager(userRepositoryMock.Object, jwtServiceMock.Object, refreshTokenRepositoryMock.Object);
        var result = await sut.LoginAsync(user.UserName, testUserPassword);
        Assert.True(result.IsSuccess);
        Assert.Equal(token, result.Value.AccessToken);
        Assert.NotEqual(token, result.Value.RefreshToken);
        refreshTokenRepositoryMock.Verify(repository => repository.AddAsync(
            user.Id,
            It.IsAny<string>(),
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_NullClaim_ShouldReturnFail()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        string userName = "testUser";
        QlarissaUser user = new() { UserName = userName };
        userRepositoryMock.Setup(x => x.GetAsync(userName)).ReturnsAsync(user);
        var sut = CreateManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.GetAsync(null!);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task GetAsync_RepositoryCallFails_ShouldReturnFail()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        var claims = new List<Claim>() { new(ClaimTypes.NameIdentifier, "user-id") };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);
        userRepositoryMock.Setup(x => x.GetByIdAsync("user-id")).ReturnsAsync(null as QlarissaUser);
        var sut = CreateManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.GetAsync(principal);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task GetAsync_RepositoryCallSucceeds_ShouldReturnOkWithUser()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        string userName = "testUser";
        var claims = new List<Claim>() { new(ClaimTypes.NameIdentifier, "user-id") };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);
        QlarissaUser user = new() { UserName = userName };
        userRepositoryMock.Setup(x => x.GetByIdAsync("user-id")).ReturnsAsync(user);
        var sut = CreateManager(userRepositoryMock.Object, jwtServiceMock.Object);
        var result = await sut.GetAsync(principal);
        Assert.True(result.IsSuccess);
        Assert.Equal(user, result.Value);
    }

    [Fact]
    public async Task RefreshAsync_RotatesRefreshTokenAndIssuesNewAccessToken()
    {
        var user = new QlarissaUser { Id = "stable-user-id", UserName = "test", Email = "test@example.com" };
        var users = new Mock<IUserRepository>();
        users.Setup(repository => repository.GetByIdAsync(user.Id)).ReturnsAsync(user);
        var jwt = new Mock<IJwtService>();
        jwt.Setup(service => service.GenerateToken(user)).Returns("new-access");
        var refreshTokens = new Mock<IRefreshTokenRepository>();
        refreshTokens
            .Setup(repository => repository.RotateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .ReturnsAsync(user.Id);

        var result = await CreateManager(users.Object, jwt.Object, refreshTokens.Object).RefreshAsync("old-refresh");

        Assert.True(result.IsSuccess);
        Assert.Equal("new-access", result.Value.AccessToken);
        Assert.NotEqual("old-refresh", result.Value.RefreshToken);
        refreshTokens.Verify(repository => repository.RotateAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_RejectsInvalidRefreshToken()
    {
        var refreshTokens = new Mock<IRefreshTokenRepository>();
        refreshTokens
            .Setup(repository => repository.RotateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .ReturnsAsync((string?)null);

        var result = await CreateManager(
            Mock.Of<IUserRepository>(),
            Mock.Of<IJwtService>(),
            refreshTokens.Object).RefreshAsync("invalid");

        Assert.True(result.IsFailed);
    }
}