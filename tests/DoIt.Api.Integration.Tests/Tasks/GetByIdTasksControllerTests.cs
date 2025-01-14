using DoIt.Api.Controllers.Tasks;
using System.Net;
using System.Net.Http.Json;

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
            new CreateTaskRequest("Task with title 1")
        );

        var firstTaskId = firstTaskResult!.Headers.Location!.Segments[3];

        var secondTaskResult = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest("Task with title 2")
        );

        // Act
        var result = await _client.GetAsync($"api/tasks/{firstTaskId}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeTrue();

        var parsedContent = await result.Content.ReadFromJsonAsync<GetTaskResponse>();

        parsedContent
            .Should()
            .Match<GetTaskResponse>(
                t => t.Title == "Task with title 1"
                  && t.Id == Guid.Parse(firstTaskId)
            );
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
