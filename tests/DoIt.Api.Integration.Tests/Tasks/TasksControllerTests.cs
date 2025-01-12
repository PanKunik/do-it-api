using DoIt.Api.Controllers.Tasks;
using System.Net.Http.Json;

namespace DoIt.Api.Integration.Tests.Tasks;

public class TasksControllerTests
    : IClassFixture<DoItApiFactory>
{
    private readonly HttpClient _client;

    public TasksControllerTests(DoItApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact(Skip = "Not able to create tasks in db")]
    public async Task Get_WhenInvoked_ShouldReturnAllTasksFromDatabase()
    {
        // Act
        var result = await _client.GetFromJsonAsync<List<GetTaskResponse>>("api/tasks");

        // Assert
        result
            .Should()
            .NotBeEmpty();
    }

    // TODO: Add test for GetById, Get, Create and Delete
}
