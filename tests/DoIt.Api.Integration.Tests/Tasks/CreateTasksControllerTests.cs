using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Services.Tasks;
using System.Net;
using System.Net.Http.Json;
using Constants = DoIt.Api.TestUtils.Constants;

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
        var response = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest(Constants.Tasks.Title.Value)
        );

        var responseContent = await response.Content.ReadFromJsonAsync<TaskDTO>();

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        responseContent
            .Should()
            .Match<TaskDTO>(r => r.Title == Constants.Tasks.Title.Value);

        response.Headers.Location
            .Should()
            .Be($"http://localhost/api/tasks/{responseContent!.Id}");
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
