using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Tasks;

[Collection("Tasks controller tests")]
public class CreateTasksControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly ITasksRepository  _tasksRepository = apiFactory.Services.GetRequiredService<ITasksRepository>();
    
    [Fact]
    public async Task Create_WhenInvokedWithProperData_ShouldSaveInDatabase()
    {
        await TaskBuilder.Default().SaveInRepository(_tasksRepository);
        await TaskBuilder.Default(123).SaveInRepository(_tasksRepository);
        await TaskBuilder.Default(2).WithId(Guid.NewGuid()).SaveInRepository(_tasksRepository);
        // Act
        var response = await _client.PostAsJsonAsync(
            "api/tasks",
            new CreateTaskRequest(
                Constants.Tasks.Title.Value,
                IsImportant: null,
                TaskListId: null
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

        var tasksInDatabase = await _tasksRepository.GetAll();
        tasksInDatabase
            .Should()
            .HaveCount(1);
    }

    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public Task DisposeAsync() => _resetDatabase();
}