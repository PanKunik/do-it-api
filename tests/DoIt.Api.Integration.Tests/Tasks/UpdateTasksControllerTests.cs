using DoIt.Api.Controllers.Tasks;
using System.Net;
using System.Net.Http.Json;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class UpdateTasksControllerTests
    : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public UpdateTasksControllerTests(DoItApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Update_WhenInvokedForExistingTask_ShouldReturnNoContentResult()
    {
        // Arrange
        var createTaskResponse = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest("Test title 1")
        );

        var createdTaskId = createTaskResponse.Headers.Location!.Segments[3];

        // Act
        var response = await _client.PutAsJsonAsync(
            $"api/tasks/{createdTaskId}",
            new UpdateTaskRequest("Test title 2")
        );

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Update_WhenTaskNotFound_ShouldReturn404NotFound()
    {
        // Act
        var response = await _client.PutAsJsonAsync(
            $"api/tasks/{Guid.NewGuid()}",
            new UpdateTaskRequest("Test title 2")
        );

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
