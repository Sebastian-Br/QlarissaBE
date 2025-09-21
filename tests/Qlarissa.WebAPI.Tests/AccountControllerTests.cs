using Microsoft.AspNetCore.Mvc;
using Moq;
using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Controllers;
using Qlarissa.WebAPI.Models;

namespace Qlarissa.WebAPI.Tests;

public class AccountControllerTests
{
    [Fact]
    public void DependencyIsMissing_ShouldThrow()
    {
        var qlarissaUserManagerMock = new Mock<IQlarissaUserManager>();
        Assert.Throws<ArgumentNullException>(() => new AccountController(null!));
        Assert.NotNull(new AccountController(qlarissaUserManagerMock.Object));
    }

    [Fact]
    public async Task RegisterAsync_Succeeds_ShouldReturnOk()
    {
        var qlarissaUserManagerMock = new Mock<IQlarissaUserManager>();

        var request = new RegisterUserRequest
        {
            Username = "test",
            Email = "test@test.com",
            Password = "Test1!"
        };

        qlarissaUserManagerMock.Setup(mgr => mgr.RegisterAsync(request.Username, request.Email, request.Password)).ReturnsAsync(FluentResults.Result.Ok());
        var controller = new AccountController(qlarissaUserManagerMock.Object);
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
            Password = "Test1!"
        };

        qlarissaUserManagerMock.Setup(mgr => mgr.RegisterAsync(request.Username, request.Email, request.Password)).ReturnsAsync(FluentResults.Result.Fail(""));
        var controller = new AccountController(qlarissaUserManagerMock.Object);
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

        qlarissaUserManagerMock.Setup(mgr => mgr.LoginAsync(request.Username, request.Password)).ReturnsAsync(FluentResults.Result.Ok());
        var controller = new AccountController(qlarissaUserManagerMock.Object);
        var result = await controller.LoginAsync(request);
        Assert.IsType<OkObjectResult>(result);
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

        qlarissaUserManagerMock.Setup(mgr => mgr.LoginAsync(request.Username, request.Password)).ReturnsAsync(FluentResults.Result.Fail(""));
        var controller = new AccountController(qlarissaUserManagerMock.Object);
        var result = await controller.LoginAsync(request);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}