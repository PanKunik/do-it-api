﻿using DoIt.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

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
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task HealthCheck_WhenInvoked_ShouldReturn200OKStatusCode()
    {
        // Arrange
        var cut = new AppController();

        // Act
        var result = (OkObjectResult) await cut.HealthCheck();

        // Assert
        result
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task AppCcontroller_ShouldContainApiControllerAttribute()
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
    public async Task AppCcontroller_ShouldContainRouteAttributeWithExpectedTemplate()
    {
        // Arrange
        var attribute = typeof(AppController).GetCustomAttribute<RouteAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .Be("api/[controller]");

        await Task.CompletedTask;
    }
}
