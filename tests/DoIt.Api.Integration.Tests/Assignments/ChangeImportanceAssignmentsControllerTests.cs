using System.Net;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.Assignments;

[Collection("Assignments controller tests")]
public class ChangeImportanceAssignmentsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsRepository  _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task ChangeImportance_WhenTaskDoesntExist_ShouldReturn404NotFound()
    {
        // Act
        var response = await _client.PutAsync(
            $"api/assignments/{Guid.NewGuid()}/change-importance",
            null
        );
        
        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ChangeImportance_WhenAssignmentExists_ShouldChangeImportance()
    {
        // Arrange
        await AssignmentBuilder.Default().WithIsImportant(true).SaveInRepository(_assignmentsRepository);
        var savedAssignment = (await _assignmentsRepository.GetAll()).First();
        
        // Act
        await _client.PutAsync(
            $"api/assignments/{savedAssignment.Id.Value}/change-importance",
            null
        );
        
        // Assert
        var updatedAssignment = (await _assignmentsRepository.GetById(savedAssignment.Id)).Value!;
        updatedAssignment.IsImportant
            .Should()
            .BeFalse();

    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}