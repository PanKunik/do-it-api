using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Services.AssignmentsLists;
using DoIt.Api.Services.Assignments;
using DoIt.Api.TestUtils.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DoIt.Api.Integration.Tests.AssignmentsLists;

[Collection("Assignments controller tests")]
public class GetByIdAssignmentsListsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsListsRepository  _assignmentsListsRepository = apiFactory.Services.GetRequiredService<IAssignmentsListsRepository>();
    private readonly IAssignmentsRepository  _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();

    [Fact]
    public async Task GetById_WhenAssignmentsListDoesntExist_ShouldReturn404NotFound()
    {
        // Act
        var result = await _client.GetAsync($"api/assignments-lists/{Guid.NewGuid()}");
        
        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeFalse();
        
        result.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_WhenAssignmentsListExist_ShouldReturnOnlyOneAssignmentsList()
    {
        // Arrange
        for (int i = 0; i < 10; i++)
            await AssignmentsListBuilder.Default(i + 1).SaveInRepository(_assignmentsListsRepository);

        // TODO: Write helper method
        var expectedAssignmentsList = (await _assignmentsListsRepository.GetById(AssignmentsListId.CreateFrom(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)).Value!)).Value!;

        for (int i = 0; i < 3; i++)
            await AssignmentBuilder.Default(i + 1)
                .WithAssignmentsListId(expectedAssignmentsList.Id.Value)
                .SaveInRepository(_assignmentsRepository);

        var expectedAssignments = await _assignmentsRepository.GetAll();
        
        // Act
        var result = await _client.GetAsync($"api/assignments-lists/{expectedAssignmentsList.Id.Value}");
        
        // Assert
        result.IsSuccessStatusCode
            .Should()
            .BeTrue();

        var parsedContent = await result.Content.ReadFromJsonAsync<AssignmentsListDto>();

        parsedContent
            .Should()
            .Match<AssignmentsListDto>(
                l => l.Id == expectedAssignmentsList.Id.Value
                  && l.Name ==  expectedAssignmentsList.Name.Value
                  && l.CreatedAt == expectedAssignmentsList.CreatedAt
                  && l.Assignments!.SequenceEqual(expectedAssignments.Select(t => t.ToDto()))
            );
    }
    
    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}