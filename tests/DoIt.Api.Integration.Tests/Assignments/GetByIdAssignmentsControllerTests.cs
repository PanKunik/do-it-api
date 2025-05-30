using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Services.Assignments;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.Assignments;

[Collection("Assignments controller tests")]
public class GetByIdAssignmentsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsRepository _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task GetById_WhenAssignmentDoesntExist_ShouldReturn404NotFound()
    {
        // Act
        var result = await _client.GetAsync($"api/assignments/{Guid.NewGuid()}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeFalse();

        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_WhenAssignmentsExist_ShouldReturnOnlyOneAssignment()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await AssignmentBuilder.Default(i).SaveInRepository(_assignmentsRepository);
            
        var expectedAssignment = (await _assignmentsRepository.GetAll()).Last();

        // Act
        var result = await _client.GetAsync($"api/assignments/{expectedAssignment.Id.Value}");

        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeTrue();

        var parsedContent = await result.Content.ReadFromJsonAsync<AssignmentDto>();

        parsedContent
            .Should()
            .Match<AssignmentDto>(
                t => t.Title == expectedAssignment.Title.Value
                  && t.Id == expectedAssignment.Id.Value
                  && t.IsDone == expectedAssignment.IsDone
                  && t.IsImportant == expectedAssignment.IsImportant
                  && t.CreatedAt == expectedAssignment.CreatedAt
            );
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}