using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Persistence.Repositories.TaskLists;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.Services.TaskLists;
using DoIt.Api.Services.Tasks;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.TaskLists;

[Collection("Tasks controller tests")]
public class GetByIdTaskListsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly ITaskListsRepository  _taskListsRepository = apiFactory.Services.GetRequiredService<ITaskListsRepository>();
    private readonly ITasksRepository  _tasksRepository = apiFactory.Services.GetRequiredService<ITasksRepository>();

    [Fact]
    public async Task GetById_WhenTaskListDoesntExist_ShouldReturn404NotFound()
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
    public async Task GetById_WhenTaskListsExit_ShouldReturnOnlyOneTaskList()
    {
        // Arrange
        for (int i = 0; i < 10; i++)
            await TaskListBuilder.Default(i + 1).SaveInRepository(_taskListsRepository);

        var expectedTaskList = (await _taskListsRepository.GetById(TaskListId.CreateFrom(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)).Value!)).Value!;

        for (int i = 0; i < 3; i++)
            await TaskBuilder.Default(i + 1)
                .WithTaskListId(expectedTaskList.Id.Value)
                .SaveInRepository(_tasksRepository);

        var expectedTasks = await _tasksRepository.GetAll();
        
        // Act
        var result = await _client.GetAsync($"api/task-lists/{expectedTaskList.Id.Value}");
        
        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeTrue();

        var parsedContent = await result.Content.ReadFromJsonAsync<TaskListDto>();

        parsedContent
            .Should()
            .Match<TaskListDto>(
                l => l.Id == expectedTaskList.Id.Value
                  && l.Name ==  expectedTaskList.Name.Value
                  && l.CreatedAt == expectedTaskList.CreatedAt
                  && l.Tasks!.SequenceEqual(expectedTasks.Select(t => t.ToDto()))
            );
    }
    
    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}