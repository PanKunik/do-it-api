﻿using DoIt.Api.Controllers.Tasks;
using System.Net.Http.Json;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class DeleteTasksControllerTests
    : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public DeleteTasksControllerTests(DoItApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Delete_WhenInvokedForOneTask_ShouldDeleteSpecifiedTaskFromDatabase()
    {
        // Arrange
        var firstTaskResponse = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest("First task")
        );

        var firstTaskId = firstTaskResponse!.Headers.Location!.Segments[3];

        var secondTaskResponse = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest("Second task")
        );

        var secondTaskId = secondTaskResponse!.Headers.Location!.Segments[3];

        var tasksInDatabase = await _client.GetFromJsonAsync<List<GetTaskResponse>>("api/tasks");

        tasksInDatabase
            .Should()
            .HaveCount(2);

        // Act
        var result = await _client.DeleteAsync($"api/tasks/{firstTaskId}");

        // Assert
        tasksInDatabase = await _client.GetFromJsonAsync<List<GetTaskResponse>>("api/tasks");

        tasksInDatabase
            .Should()
            .HaveCount(1);


        tasksInDatabase!
            .First().Title
            .Should()
            .BeEquivalentTo("Second task");
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
