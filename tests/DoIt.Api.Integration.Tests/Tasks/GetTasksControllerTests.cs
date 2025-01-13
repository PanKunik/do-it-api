using DoIt.Api.Controllers.Tasks;
using System.Net.Http.Json;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class GetTasksControllerTests
    : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public GetTasksControllerTests(DoItApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Get_WhenInvoked_ShouldReturnAllTasksFromDatabase()
    {
        // Arrange
        await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest("Task with title")
        );

        // Act
        var result = await _client.GetFromJsonAsync<List<GetTaskResponse>>("api/tasks");

        // Assert
        result
            .Should()
            .NotBeEmpty();

        result
            .Should()
            .HaveCount(1);

        result!
            .First().Title
            .Should()
            .BeEquivalentTo("Task with title");
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
