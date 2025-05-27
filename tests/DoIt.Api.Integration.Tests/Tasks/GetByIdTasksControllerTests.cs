using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.Services.Tasks;
using DoIt.Api.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class GetByIdTasksControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly ITasksRepository _tasksRepository = apiFactory.Services.GetRequiredService<ITasksRepository>();

    [Fact]
    public async Task GetById_WhenTaskDoesntExist_ShouldReturn404NotFound()
    {
        // Act
        var result = await _client.GetAsync($"api/tasks/{Guid.NewGuid()}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeFalse();

        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_WhenTasksExists_ShouldReturnOnlyOneTask()
    {
        // Arrange
        var tasks = TaskListsUtilities.CreateTasks(2);
        foreach (var task in tasks)
            await _tasksRepository.Create(task);

        var expectedTask = tasks.Last();

        // Act
        var result = await _client.GetAsync($"api/tasks/{expectedTask.Id.Value}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeTrue();

        var parsedContent = await result.Content.ReadFromJsonAsync<TaskDto>();

        parsedContent
            .Should()
            .Match<TaskDto>(
                t => t.Title == expectedTask.Title.Value
                  && t.Id == expectedTask.Id.Value
                  && t.IsDone == expectedTask.IsDone
                  && t.IsImportant == expectedTask.IsImportant
            );
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}