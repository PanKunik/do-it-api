using DoIt.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DoIt.Api.Unit.Tests.Controllers;
public class AppControllerTests
{
    [Fact]
    public async Task HealthCheck_WhenInvoked_ShouldReturnOkResult()
    {
        // Arrange
        var cut = new AppController();

        // Act
        var result = await cut.HealthCheck();

        // Assert
        result
            .Should()
            .BeOfType<OkResult>();
    }

    [Fact]
    public async Task HealthCheck_WhenInvoked_ShouldReturn200OKStatusCode()
    {
        // Arrange
        var cut = new AppController();

        // Act
        var result = (OkResult)await cut.HealthCheck();

        // Assert
        result
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.OK);
    }
}
