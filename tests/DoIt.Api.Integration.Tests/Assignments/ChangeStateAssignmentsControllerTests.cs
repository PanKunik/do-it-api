using System.Net;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.Assignments;

[Collection("Assignments controller tests")]
public class ChangeStateAssignmentsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsRepository  _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task ChangeState_WhenAssignmentDoesntExist_ShouldReturn404NotFound()
    {
        // Act
        var response = await _client.PutAsync(
            $"api/assignments/{Guid.NewGuid()}/change-state",
            null
        );
        
        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ChangeState_WhenAssignmentExists_ShouldChangeState()
    {
        // Arrange
        await AssignmentBuilder.Default().WithIsDone(false).SaveInRepository(_assignmentsRepository);
        var savedAssignment = (await _assignmentsRepository.GetAll()).First();
        
        // Act
        await _client.PutAsync(
            $"api/assignments/{savedAssignment.Id.Value}/change-state",
            null
        );
        
        // Assert
        var updatedAssignment = (await _assignmentsRepository.GetById(savedAssignment.Id)).Value!;
        updatedAssignment.IsDone
            .Should()
            .BeTrue();

    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}