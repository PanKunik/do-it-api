using DoIt.Api.Controllers.Tasks;
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

        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        responseContent
            .Should()
            .BeEmpty();

        response.Headers.Location
            .Should()
            .NotBeNull();
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
