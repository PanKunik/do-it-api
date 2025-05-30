using System.Net;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.AssignmentsLists;

[Collection("Assignments controller tests")]
public class AttachAssignmentAssignmentsListsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsListsRepository  _assignmentsListsRepository = apiFactory.Services.GetRequiredService<IAssignmentsListsRepository>();
    private readonly IAssignmentsRepository  _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task AttachAssignment_WhenAssignmentNotFound_ShouldReturn404NotFound()
    {
        // Arrange
        var assignmentsList = AssignmentsListBuilder.Default().Build();
        await _assignmentsListsRepository.Create(assignmentsList);
        
        // Act
        var response = await _client.PutAsync(
            $"api/assignments-list/{assignmentsList.Id.Value}/attach/{Guid.NewGuid()}",
            null
        );

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task AttachAssignment_WhenAssignmentsListNotFound_ShouldReturn404NotFound()
    {
        // Arrange
        var assignment = AssignmentBuilder.Default().Build();
        await _assignmentsRepository.Create(assignment);
        
        // Act
        var response = await _client.PutAsync(
            $"api/assignments-list/{Guid.NewGuid()}/attach/{assignment.Id.Value}",
            null
        );

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AttachAssignment_WhenInvokedForExistingAssignmentAndAssignmentsList_ShouldAttachAssignmentToList()
    {
        // Arrange
        var assignment = AssignmentBuilder.Default().Build();
        await _assignmentsRepository.Create(assignment);
        
        var assignmentsList = AssignmentsListBuilder.Default().Build();
        await _assignmentsListsRepository.Create(assignmentsList);        
        
        // Act
        await _client.PutAsync(
            $"api/assignments-lists/{assignmentsList.Id.Value}/attach/{assignment.Id.Value}",
            null
        );
        
        // Assert
        var assignmentsListInDatabase = (await _assignmentsListsRepository.GetById(assignmentsList.Id)).Value!;

        assignmentsListInDatabase.Assignments
            .Should()
            .HaveCount(1);
        
        assignmentsListInDatabase.Assignments
            .SequenceEqual([assignment])
            .Should()
            .BeTrue();
    }
    
    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}