using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Services.Assignments;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.Assignments;

[Collection("Assignments controller tests")]
public class GetAssignmentsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsRepository _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task Get_WithoutAnyAssignments_ShouldReturnEmptyListResponse()
    {
        // Act
        var response = await _client.GetAsync("api/assignments");
        var responseContent = await response.Content.ReadFromJsonAsync<List<AssignmentDto>>();

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        responseContent
            .Should()
            .BeEquivalentTo(new List<AssignmentDto>());
    }

    [Fact]
    public async Task Get_WhenAssignmentsExist_ShouldReturnListOfAllAssignments()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await AssignmentBuilder.Default(i).SaveInRepository(_assignmentsRepository);
        
        var expectedAssignments = await _assignmentsRepository.GetAll();

        // Act
        var response = await _client.GetAsync("api/assignments");
        var responseContent = await response.Content.ReadFromJsonAsync<List<AssignmentDto>>();

        // Assert
        response.IsSuccessStatusCode
            .Should()
            .BeTrue();
        
        responseContent
            .Should()
            .BeEquivalentTo(expectedAssignments.Select(t => t.ToDto()));
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}