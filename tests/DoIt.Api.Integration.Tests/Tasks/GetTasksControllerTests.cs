using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Services.Tasks;
using System.Net;
using System.Net.Http.Json;
using Constants = DoIt.Api.TestUtils.Constants;

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
    public async Task Get_WithoutTasks_ShouldReturnEmptyListResponse()
    {
        // Act
        var response = await _client.GetAsync("api/tasks");
        var responseContent = await response.Content.ReadFromJsonAsync<List<TaskDto>>();

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        responseContent
            .Should()
            .BeEquivalentTo(new List<TaskDto>());
    }

    [Fact]
    public async Task Get_WhenTasksExists_ShouldReturnListOfAllTasks()
    {
        // Arrange
        await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest(Constants.Tasks.TitleFromIndex(1).Value, null)
        );

        await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest(Constants.Tasks.TitleFromIndex(2).Value, null)
        );

        // Act
        var response = await _client.GetAsync("api/tasks");
        var responseContent = await response.Content.ReadFromJsonAsync<List<TaskDto>>();

        // Assert
        response.IsSuccessStatusCode
            .Should()
            .BeTrue();

        responseContent
            .Should()
            .HaveCount(2);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
