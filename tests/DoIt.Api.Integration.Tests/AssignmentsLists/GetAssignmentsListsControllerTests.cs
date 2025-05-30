using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.Services.AssignmentsLists;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.AssignmentsLists;

[Collection("Assignments controller tests")]
public class GetAssignmentsListsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsListsRepository  _assignmentsListsRepository = apiFactory.Services.GetRequiredService<IAssignmentsListsRepository>();

    [Fact]
    public async Task Get_WithoutAnyAssignmentsLists_ShouldReturnEmptyListResponse()
    {
        // Act
        var response = await _client.GetAsync("api/assignments-lists");
        var responseContent = await response.Content.ReadFromJsonAsync<List<AssignmentsListDto>>();
        
        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        responseContent
            .Should()
            .BeEquivalentTo(new List<AssignmentsListDto>());
    }
    
    [Fact]
    public async Task Get_WhenAssignmentsListsExist_ShouldReturnListOfAllAssignmentsLists()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await AssignmentsListBuilder.Default(i).SaveInRepository(_assignmentsListsRepository);
        
        var expectedAssignmentsLists = await _assignmentsListsRepository.GetAll();

        // Act
        var response = await _client.GetAsync("api/assignments-lists");
        var responseContent = await response.Content.ReadFromJsonAsync<List<AssignmentsListDto>>();

        // Assert
        response.IsSuccessStatusCode
            .Should()
            .BeTrue();
        
        responseContent
            .Should()
            .BeEquivalentTo(expectedAssignmentsLists.Select(t => t.ToDto()));
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}