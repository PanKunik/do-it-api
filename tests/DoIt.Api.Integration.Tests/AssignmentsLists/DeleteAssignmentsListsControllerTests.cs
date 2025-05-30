using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.AssignmentsLists;
using DoIt.Api.Domain;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.TestUtils;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.AssignmentsLists;

[Collection("Assignments controller tests")]
public class DeleteAssignmentsListsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsListsRepository  _assignmentsListsRepository = apiFactory.Services.GetRequiredService<IAssignmentsListsRepository>();
    private readonly IAssignmentsRepository  _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task Delete_WhenInvokedWithProperData_ShouldDeleteAssignmentsListAndDetachAllAssignmentsFromList()
    {
        // Arrange
        var assignmentsList = AssignmentsListBuilder.Default().Build();
        await _assignmentsListsRepository.Create(assignmentsList);
        
        var assignments = Enumerable.Range(0, 5)
            .Select(r => AssignmentBuilder.Default(r + 1).WithAssignmentsListId(assignmentsList.Id.Value).Build())
            .ToList();
        
        foreach (var assignment in assignments)
            await _assignmentsRepository.Create(assignment);
        
        // Act
        await _client.DeleteAsync($"api/assignments-lists/{assignmentsList.Id.Value}");
        
        // Assert
        var getAssignmentsListByIdResult = await _assignmentsListsRepository.GetById(assignmentsList.Id);
        getAssignmentsListByIdResult.Error!
            .Should()
            .Be(Errors.AssignmentsList.NotFound);
        
        var assignmentsInDatabase = await _assignmentsRepository.GetAll();
        assignmentsInDatabase
            .All(a => a.AssignmentsListId is null)
            .Should()
            .BeTrue();
    }
    
    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}