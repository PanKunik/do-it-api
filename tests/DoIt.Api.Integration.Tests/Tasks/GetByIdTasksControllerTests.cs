using DoIt.Api.Controllers.Tasks;
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
    public async Task GetById_WhenInvoked_ShouldReturnAllExpectedFromDatabase()
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
        var result = await _client.GetFromJsonAsync<GetTaskResponse>($"api/tasks/{firstTaskId}");

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Id
            .Should()
            .Be(Guid.Parse(firstTaskId));

        result.Title
            .Should()
            .Be("Task with title 1");
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
