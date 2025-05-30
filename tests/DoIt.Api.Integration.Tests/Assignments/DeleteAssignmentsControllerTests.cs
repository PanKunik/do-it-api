using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Services.Assignments;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.Assignments;

[Collection("Assignments controller tests")]
public class DeleteAssignmentsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsRepository _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task Delete_WhenAssignmentsExist_ShouldDeleteExpectedAssignment()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await AssignmentBuilder.Default(i).SaveInRepository(_assignmentsRepository);
        
        var assignmentToDelete = (await _assignmentsRepository.GetAll()).Last();

        var assignmentsInDatabase = await _client.GetFromJsonAsync<List<AssignmentDto>>("api/assignments");

        assignmentsInDatabase
            .Should()
            .HaveCount(5);

        // Act
        var result = await _client.DeleteAsync($"api/assignments/{assignmentToDelete.Id.Value}");

        // Assert
        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        (await result.Content.ReadAsStringAsync())
            .Should()
            .BeEmpty();

        assignmentsInDatabase = await _client.GetFromJsonAsync<List<AssignmentDto>>("api/assignments");

        assignmentsInDatabase
            .Should()
            .HaveCount(4);
    }

    [Fact]
    public async Task Delete_WhenAssignmentDoesntExist_ShouldReturn404NotFound()
    {
        // Act
        var result = await _client.DeleteAsync($"api/assignments/{Guid.NewGuid()}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeFalse();

        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}