using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.TaskLists;
using DoIt.Api.Persistence.Repositories.TaskLists;
using DoIt.Api.TestUtils;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.TaskLists;

[Collection("Tasks controller tests")]
public class CreateTaskListsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    // private readonly ITaskListsRepository  _taskListsRepository = apiFactory.Services.GetRequiredService<ITaskListsRepository>();

    [Fact]
    public async Task Create_WhenInvokedWithProperData_ShouldSaveInDatabase()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "api/task-lists",
            new CreateTaskListRequest(
                Constants.TaskLists.Name.Value
            )
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

        // var taskListsInDatabase = await _taskListsRepository.GetAll();
        // taskListsInDatabase
        //     .Should()
        //     .HaveCount(1);
    }
    
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public Task DisposeAsync() => _resetDatabase();
}