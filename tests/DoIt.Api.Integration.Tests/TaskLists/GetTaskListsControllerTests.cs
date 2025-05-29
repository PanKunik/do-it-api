using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Persistence.Repositories.TaskLists;
using DoIt.Api.Services.TaskLists;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.TaskLists;

[Collection("Tasks controller tests")]
public class GetTaskListsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly ITaskListsRepository  _taskListsRepository = apiFactory.Services.GetRequiredService<ITaskListsRepository>();

    [Fact]
    public async Task Get_WithoutAnyTaskLists_ShouldReturnEmptyListResponse()
    {
        // Act
        var response = await _client.GetAsync("api/task-lists");
        var responseContent = await response.Content.ReadFromJsonAsync<List<TaskListDto>>();
        
        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        responseContent
            .Should()
            .BeEquivalentTo(new List<TaskListDto>());
    }
    
    [Fact]
    public async Task Get_WhenTaskListsExist_ShouldReturnListOfAllTaskLists()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await TaskListBuilder.Default(i).SaveInRepository(_taskListsRepository);
        
        var expectedTaskLists = await _taskListsRepository.GetAll();

        // Act
        var response = await _client.GetAsync("api/task-lists");
        var responseContent = await response.Content.ReadFromJsonAsync<List<TaskListDto>>();

        // Assert
        response.IsSuccessStatusCode
            .Should()
            .BeTrue();
        
        responseContent
            .Should()
            .BeEquivalentTo(expectedTaskLists.Select(t => t.ToDto()));
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}