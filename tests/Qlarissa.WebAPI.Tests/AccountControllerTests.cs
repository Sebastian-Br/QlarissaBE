using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Qlarissa.Application.Models;
using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Settings;
using Qlarissa.WebAPI.Controllers;
using Qlarissa.WebAPI.Models;

namespace Qlarissa.WebAPI.Tests;

public class AccountControllerTests
{
    private static AccountController CreateController(Mock<IQlarissaUserManager> manager)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cors:AllowedOrigins:0"] = "https://frontend.example"
            })
            .Build();
        var controller = new AccountController(
            manager.Object,
            Options.Create(new AuthTokenOptions()),
            configuration);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.HttpContext.Request.Headers.Origin = "https://frontend.example";
        return controller;
    }

    [Fact]
    public void DependencyIsMissing_ShouldThrow()
    {
        var qlarissaUserManagerMock = new Mock<IQlarissaUserManager>();
        var options = Options.Create(new AuthTokenOptions());
        var configuration = new ConfigurationBuilder().Build();
        Assert.Throws<ArgumentNullException>(() => new AccountController(null!, options, configuration));
        Assert.NotNull(new AccountController(qlarissaUserManagerMock.Object, options, configuration));
    }

    [Fact]
    public async Task RegisterAsync_Succeeds_ShouldReturnOk()
    {
        var qlarissaUserManagerMock = new Mock<IQlarissaUserManager>();

        var request = new RegisterUserRequest
        {
            Username = "test",
            Email = "test@test.com",
            DisplayCurrencyId = 1,
            Password = "Test1!"
        };

        qlarissaUserManagerMock.Setup(mgr => mgr.RegisterAsync(request.Username, request.Email, request.DisplayCurrencyId, request.Password)).ReturnsAsync(FluentResults.Result.Ok());
        var controller = CreateController(qlarissaUserManagerMock);
        var result = await controller.RegisterAsync(request);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task RegisterAsync_Fails_ShouldReturnbadRequest()
    {
        var qlarissaUserManagerMock = new Mock<IQlarissaUserManager>();

        var request = new RegisterUserRequest
        {
            Username = "test",
            Email = "test@test.com",
            DisplayCurrencyId = 1,
            Password = "Test1!"
        };

        qlarissaUserManagerMock.Setup(mgr => mgr.RegisterAsync(request.Username, request.Email, request.DisplayCurrencyId, request.Password)).ReturnsAsync(FluentResults.Result.Fail(""));
        var controller = CreateController(qlarissaUserManagerMock);
        var result = await controller.RegisterAsync(request);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_Succeeds_ShouldReturnOk()
    {
        var qlarissaUserManagerMock = new Mock<IQlarissaUserManager>();

        var request = new UserLoginRequest
        {
            Username = "test",
            Password = "Test1!"
        };

        qlarissaUserManagerMock.Setup(mgr => mgr.LoginAsync(request.Username, request.Password)).ReturnsAsync(
            FluentResults.Result.Ok(new AuthenticationSession("access-token", "refresh-token")));
        var controller = CreateController(qlarissaUserManagerMock);
        var result = await controller.LoginAsync(request);
        Assert.IsType<OkObjectResult>(result);
        Assert.Contains("HttpOnly", controller.Response.Headers.SetCookie.ToString(), StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Secure", controller.Response.Headers.SetCookie.ToString(), StringComparison.OrdinalIgnoreCase);
        Assert.Contains("SameSite=None", controller.Response.Headers.SetCookie.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LoginAsync_Fails_ShouldReturnUnauthorized()
    {
        var qlarissaUserManagerMock = new Mock<IQlarissaUserManager>();

        var request = new UserLoginRequest
        {
            Username = "test",
            Password = "Test1!"
        };

        qlarissaUserManagerMock.Setup(mgr => mgr.LoginAsync(request.Username, request.Password)).ReturnsAsync(FluentResults.Result.Fail<AuthenticationSession>(""));
        var controller = CreateController(qlarissaUserManagerMock);
        var result = await controller.LoginAsync(request);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task RefreshAsync_SucceedsAndRotatesHttpOnlyCookie()
    {
        var manager = new Mock<IQlarissaUserManager>();
        manager.Setup(item => item.RefreshAsync("old-refresh"))
            .ReturnsAsync(FluentResults.Result.Ok(new AuthenticationSession("new-access", "new-refresh")));
        var controller = CreateController(manager);
        controller.HttpContext.Request.Headers.Cookie = "qlarissa.refresh=old-refresh";

        var result = await controller.RefreshAsync();

        Assert.IsType<OkObjectResult>(result);
        Assert.Contains("new-refresh", controller.Response.Headers.SetCookie.ToString());
        manager.Verify(item => item.RefreshAsync("old-refresh"), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_RejectsUntrustedOrigin()
    {
        var manager = new Mock<IQlarissaUserManager>();
        var controller = CreateController(manager);
        controller.HttpContext.Request.Headers.Origin = "https://attacker.example";

        var result = await controller.RefreshAsync();

        Assert.IsType<ForbidResult>(result);
        manager.Verify(item => item.RefreshAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RefreshAsync_FailureClearsRefreshCookie()
    {
        var manager = new Mock<IQlarissaUserManager>();
        manager.Setup(item => item.RefreshAsync("expired-refresh"))
            .ReturnsAsync(FluentResults.Result.Fail<AuthenticationSession>("expired"));
        var controller = CreateController(manager);
        controller.HttpContext.Request.Headers.Cookie = "qlarissa.refresh=expired-refresh";

        var result = await controller.RefreshAsync();

        Assert.IsType<UnauthorizedResult>(result);
        Assert.Contains("expires=", controller.Response.Headers.SetCookie.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LogoutAsync_RevokesRefreshTokenAndClearsCookie()
    {
        var manager = new Mock<IQlarissaUserManager>();
        manager.Setup(item => item.RevokeRefreshTokenAsync("current-refresh"))
            .Returns(Task.CompletedTask);
        var controller = CreateController(manager);
        controller.HttpContext.Request.Headers.Cookie = "qlarissa.refresh=current-refresh";

        var result = await controller.LogoutAsync();

        Assert.IsType<NoContentResult>(result);
        manager.Verify(item => item.RevokeRefreshTokenAsync("current-refresh"), Times.Once);
        Assert.Contains("expires=", controller.Response.Headers.SetCookie.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}