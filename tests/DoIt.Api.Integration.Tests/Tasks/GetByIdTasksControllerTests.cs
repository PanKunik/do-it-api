﻿using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Services.Tasks;
using System.Net;
using System.Net.Http.Json;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class GetByIdTasksControllerTests
    : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public GetByIdTasksControllerTests(DoItApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task GetById_WhenTaskDoesntExists_ShouldReturn404NotFound()
    {
        // Act
        var result = await _client.GetAsync($"api/tasks/{Guid.NewGuid()}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeFalse();

        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_WhenTasksExists_ShouldReturnOnlyOneTask()
    {
        // Arrange
        var firstTaskResult = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest(Constants.Tasks.TitleFromIndex(1).Value, null)
        );

        var firstTaskId = firstTaskResult!.Headers.Location!.Segments[3];

        var secondTaskResult = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest(Constants.Tasks.TitleFromIndex(2).Value, null)
        );

        // Act
        var result = await _client.GetAsync($"api/tasks/{firstTaskId}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeTrue();

        var parsedContent = await result.Content.ReadFromJsonAsync<TaskDto>();

        parsedContent
            .Should()
            .Match<TaskDto>(
                t => t.Title == Constants.Tasks.TitleFromIndex(1).Value
                  && t.Id == Guid.Parse(firstTaskId)
            );
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
