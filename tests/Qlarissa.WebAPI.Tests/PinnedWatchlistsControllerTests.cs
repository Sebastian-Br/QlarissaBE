using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Qlarissa.Application.Interfaces;
using Qlarissa.Domain;
using Qlarissa.Domain.Watchlists;
using Qlarissa.WebAPI.Controllers;

namespace Qlarissa.WebAPI.Tests;

public sealed class PinnedWatchlistsControllerTests
{
    private static WatchlistController CreateController(
        Mock<IQlarissaUserManager> userManager,
        Mock<IWatchlistManager> watchlistManager)
    {
        userManager
            .Setup(manager => manager.GetAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(Result.Ok(new QlarissaUser { Id = "user-1" }));

        return new WatchlistController(userManager.Object, watchlistManager.Object);
    }

    [Fact]
    public async Task GetPinnedWatchlistsAsync_UsesAuthenticatedUserAndExistingResponseType()
    {
        var userManager = new Mock<IQlarissaUserManager>();
        var watchlistManager = new Mock<IWatchlistManager>();
        var domainWatchlist = new Watchlist
        {
            Id = 12,
            Name = "Long term",
            IsPinned = true,
            PinPosition = 3,
            WatchedSecurities =
            [
                new WatchedSecurity
                {
                    SecurityId = 24,
                    Name = "Example Corp",
                    Symbol = "EXM",
                    CurrencySymbol = "USD",
                    PreviousClosePrice = 42.5
                }
            ]
        };
        watchlistManager
            .Setup(manager => manager.GetPinnedWatchlistsAsync("user-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync([domainWatchlist]);
        var controller = CreateController(userManager, watchlistManager);

        var action = await controller.GetPinnedWatchlistsAsync(CancellationToken.None);
        var result = Assert.IsType<OkObjectResult>(action.Result);
        var response = Assert.IsAssignableFrom<IEnumerable<Models.Watchlist.Watchlist>>(result.Value);
        var returnedWatchlist = Assert.Single(response);

        Assert.Equal(domainWatchlist.Id, returnedWatchlist.Id);
        Assert.Equal(domainWatchlist.PinPosition, returnedWatchlist.PinPosition);
        var returnedSecurity = Assert.Single(returnedWatchlist.WatchedSecurities);
        Assert.Equal("EXM", returnedSecurity.Symbol);
        Assert.Equal("USD", returnedSecurity.CurrencySymbol);
        watchlistManager.Verify(
            manager => manager.GetPinnedWatchlistsAsync("user-1", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdatePinnedWatchlistOrderAsync_RejectsDuplicateIds()
    {
        var userManager = new Mock<IQlarissaUserManager>();
        var watchlistManager = new Mock<IWatchlistManager>();
        var controller = CreateController(userManager, watchlistManager);

        var result = await controller.UpdatePinnedWatchlistOrderAsync([12, 12], CancellationToken.None);

        Assert.IsType<BadRequestResult>(result);
        watchlistManager.Verify(
            manager => manager.UpdatePinnedWatchlistOrderAsync(
                It.IsAny<IReadOnlyList<int>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdatePinnedWatchlistOrderAsync_UsesAuthenticatedUser()
    {
        var userManager = new Mock<IQlarissaUserManager>();
        var watchlistManager = new Mock<IWatchlistManager>();
        watchlistManager
            .Setup(manager => manager.UpdatePinnedWatchlistOrderAsync(
                It.Is<IReadOnlyList<int>>(ids => ids.SequenceEqual(new[] { 12, 15 })),
                "user-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var controller = CreateController(userManager, watchlistManager);

        var result = await controller.UpdatePinnedWatchlistOrderAsync([12, 15], CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdatePinnedWatchlistOrderAsync_RejectsIdsOutsideUsersPinnedLists()
    {
        var userManager = new Mock<IQlarissaUserManager>();
        var watchlistManager = new Mock<IWatchlistManager>();
        watchlistManager
            .Setup(manager => manager.UpdatePinnedWatchlistOrderAsync(
                It.IsAny<IReadOnlyList<int>>(),
                "user-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var controller = CreateController(userManager, watchlistManager);

        var result = await controller.UpdatePinnedWatchlistOrderAsync([12, 15], CancellationToken.None);

        Assert.IsType<BadRequestResult>(result);
    }
}
