using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.TestUtils;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.AssignmentsLists;

[Collection("Assignments controller tests")]
public class CreateAssignmentsListsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsListsRepository  _assignmentsListsRepository = apiFactory.Services.GetRequiredService<IAssignmentsListsRepository>();

    [Fact]
    public async Task Create_WhenInvokedWithProperData_ShouldSaveInDatabase()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "api/assignments-lists",
            new CreateAssignmentsListRequest(
                Constants.AssignmentsLists.Name.Value
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

        var assignmentsListsInDatabase = await _assignmentsListsRepository.GetAll();
        assignmentsListsInDatabase
            .Should()
            .HaveCount(1);
    }
    
    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}