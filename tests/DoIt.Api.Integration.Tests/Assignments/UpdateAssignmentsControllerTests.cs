using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.Assignments;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Assignments;

[Collection("Assignments controller tests")]
public class UpdateAssignmentsControllerTests(DoItApiFactory apiFactory) : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsRepository _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task Update_WhenInvokedForExistingAssignment_ShouldReturnNoContentResult()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await AssignmentBuilder.Default(i).SaveInRepository(_assignmentsRepository);

        var assignmentToUpdate = (await _assignmentsRepository.GetAll()).Last();

        // Act
        var response = await _client.PutAsJsonAsync(
            $"api/assignments/{assignmentToUpdate.Id.Value}",
            new UpdateAssignmentRequest(Constants.Tasks.TitleFromIndex(2).Value)
        );

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        var updatedAssignment = await _assignmentsRepository.GetById(assignmentToUpdate.Id);
        updatedAssignment.Value!.Title
            .Should()
            .Be(Constants.Tasks.TitleFromIndex(2));
    }

    [Fact]
    public async Task Update_WhenAssignmentNotFound_ShouldReturn404NotFound()
    {
        // Act
        var response = await _client.PutAsJsonAsync(
            $"api/assignments/{Guid.NewGuid()}",
            new UpdateAssignmentRequest(Constants.Tasks.TitleFromIndex(2).Value)
        );

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}