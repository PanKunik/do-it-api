using DoIt.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace DoIt.Api.Unit.Tests.Controllers;

public class AppControllerTests
{
    private readonly AppController _cut = new();

    [Fact]
    public async Task HealthCheck_WhenInvoked_ShouldReturnOkResult()
    {
        // Act
        var result = await _cut.HealthCheck();

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task HealthCheck_WhenInvoked_ShouldReturn200OKStatusCode()
    {
        // Act
        var result = (OkObjectResult) await _cut.HealthCheck();

        // Assert
        result
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task AppController_ShouldContainApiControllerAttribute()
    {
        // Arrange
        var attribute = typeof(AppController).GetCustomAttribute<ApiControllerAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task AppController_ShouldContainRouteAttributeWithExpectedTemplate()
    {
        // Arrange
        var attribute = typeof(AppController).GetCustomAttribute<RouteAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .Be("api/app");

        await Task.CompletedTask;
    }
}