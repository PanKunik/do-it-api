using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class ChangeImportanceTasksControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly ITasksRepository  _tasksRepository = apiFactory.Services.GetRequiredService<ITasksRepository>();

    [Fact]
    public async Task ChangeImportance_WhenTaskDoesntExist_ShouldReturn404NotFound()
    {
        // Act
        var response = await _client.PutAsync(
            $"api/tasks/{Guid.NewGuid()}/change-importance",
            null
        );
        
        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ChangeImportance_WhenTaskExists_ShouldChangeImportance()
    {
        // Arrange
        await TaskBuilder.Default().WithIsImportant(true).SaveInRepository(_tasksRepository);
        var savedTask = (await _tasksRepository.GetAll()).First();
        
        // Act
        await _client.PutAsync(
            $"api/tasks/{savedTask.Id.Value}/change-importance",
            null
        );
        
        // Assert
        var updatedTask = (await _tasksRepository.GetById(savedTask.Id)).Value!;
        updatedTask.IsImportant
            .Should()
            .BeFalse();

    }

    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public Task DisposeAsync() => _resetDatabase();
}