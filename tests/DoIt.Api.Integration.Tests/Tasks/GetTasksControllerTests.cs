using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.Services.Tasks;
using DoIt.Api.TestUtils;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;
using Constants = DoIt.Api.TestUtils.Constants;
using Task = System.Threading.Tasks.Task;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class GetTasksControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly ITasksRepository _tasksRepository = apiFactory.Services.GetRequiredService<ITasksRepository>();

    [Fact]
    public async Task Get_WithoutAnyTasks_ShouldReturnEmptyListResponse()
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
    public async Task Get_WhenTasksExist_ShouldReturnListOfAllTasks()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await TaskBuilder.Default(i).SaveInRepository(_tasksRepository);

        // Act
        var response = await _client.GetAsync("api/tasks");
        var responseContent = await response.Content.ReadFromJsonAsync<List<TaskDto>>();

        // Assert
        response.IsSuccessStatusCode
            .Should()
            .BeTrue();

        responseContent
            .Should()
            .HaveCount(5);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}