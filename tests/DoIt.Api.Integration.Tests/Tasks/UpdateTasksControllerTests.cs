using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.TestUtils;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class UpdateTasksControllerTests(DoItApiFactory apiFactory) : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly ITasksRepository _tasksRepository = apiFactory.Services.GetRequiredService<ITasksRepository>();

    [Fact]
    public async Task Update_WhenInvokedForExistingTask_ShouldReturnNoContentResult()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await TaskBuilder.Default(i).SaveInRepository(_tasksRepository);

        var taskToUpdate = (await _tasksRepository.GetAll()).Last();

        // Act
        var response = await _client.PutAsJsonAsync(
            $"api/tasks/{taskToUpdate.Id.Value}",
            new UpdateTaskRequest(Constants.Tasks.TitleFromIndex(2).Value)
        );

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        var updatedTask = await _tasksRepository.GetById(taskToUpdate.Id);
        updatedTask.Value!.Title
            .Should()
            .Be(Constants.Tasks.TitleFromIndex(2));
    }

    [Fact]
    public async Task Update_WhenTaskNotFound_ShouldReturn404NotFound()
    {
        // Act
        var response = await _client.PutAsJsonAsync(
            $"api/tasks/{Guid.NewGuid()}",
            new UpdateTaskRequest(Constants.Tasks.TitleFromIndex(2).Value)
        );

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}