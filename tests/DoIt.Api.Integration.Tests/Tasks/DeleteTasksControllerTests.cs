using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.TaskLists;
using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.Services.Tasks;
using DoIt.Api.TestUtils;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class DeleteTasksControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly ITasksRepository _tasksRepository = apiFactory.Services.GetRequiredService<ITasksRepository>();

    [Fact]
    public async Task Delete_WhenTasksExists_ShouldDeleteExpectedTask()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await TaskBuilder.Default(i).SaveInRepository(_tasksRepository);
        
        var taskToDelete = (await _tasksRepository.GetAll()).Last();

        var tasksInDatabase = await _client.GetFromJsonAsync<List<TaskDto>>("api/tasks");

        tasksInDatabase
            .Should()
            .HaveCount(5);

        // Act
        var result = await _client.DeleteAsync($"api/tasks/{taskToDelete.Id.Value}");

        // Assert
        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        (await result.Content.ReadAsStringAsync())
            .Should()
            .BeEmpty();

        tasksInDatabase = await _client.GetFromJsonAsync<List<TaskDto>>("api/tasks");

        tasksInDatabase
            .Should()
            .HaveCount(4);
    }

    [Fact]
    public async Task Delete_WhenTaskDoesntExist_ShouldReturn404NotFound()
    {
        // Act
        var result = await _client.DeleteAsync($"api/tasks/{Guid.NewGuid()}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeFalse();

        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}