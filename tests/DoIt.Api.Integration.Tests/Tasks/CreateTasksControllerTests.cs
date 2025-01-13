using DoIt.Api.Controllers.Tasks;
using System.Net.Http.Json;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class CreateTasksControllerTests
    : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public CreateTasksControllerTests(DoItApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Create_WhenInvokedWithProperData_ShouldSaveInDatabase()
    {
        // Act
        await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest("Test title 1")
        );

        // Assert
        var tasksInDatabase = await _client.GetFromJsonAsync<List<GetTaskResponse>>("api/tasks");

        tasksInDatabase
            .Should()
            .HaveCount(1);

        tasksInDatabase!
            .First().Title
            .Should()
            .BeEquivalentTo("Test title 1");
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
