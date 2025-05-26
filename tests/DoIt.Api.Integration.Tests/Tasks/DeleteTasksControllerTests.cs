using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Services.Tasks;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class DeleteTasksControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;

    [Fact]
    public async Task Delete_WhenTasksExists_ShouldDeleteExpectedTask()
    {
        // Arrange
        var firstTaskResponse = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest(
                Constants.Tasks.TitleFromIndex(1).Value,
                IsImportant: null,
                TaskListId: null
            )
        );

        var firstTaskId = firstTaskResponse.Headers.Location!.Segments[3];

        var secondTaskResponse = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest(
                Constants.Tasks.TitleFromIndex(2).Value,
                IsImportant: null,
                TaskListId: null
            )
        );
        
        var tasksInDatabase = await _client.GetFromJsonAsync<List<TaskDto>>("api/tasks");

        tasksInDatabase
            .Should()
            .HaveCount(2);

        // Act
        var result = await _client.DeleteAsync($"api/tasks/{firstTaskId}");

        // Assert
        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        (await result.Content.ReadAsStringAsync())
            .Should()
            .BeEmpty();

        tasksInDatabase = await _client.GetFromJsonAsync<List<TaskDto>>("api/tasks");

        tasksInDatabase
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public async Task Delete_WhenTaskDoesntExists_ShouldReturn404NotFound()
    {
        // Act
        var result = await _client.DeleteAsync($"api/tasks/{Guid.NewGuid()}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeFalse();

        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}