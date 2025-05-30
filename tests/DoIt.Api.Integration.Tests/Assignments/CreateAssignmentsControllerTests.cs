using System.Net;
using System.Net.Http.Json;
using DoIt.Api.Controllers.Assignments;
using DoIt.Api.Persistence.Repositories.Assignments;
using Microsoft.Extensions.DependencyInjection;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Integration.Tests.Assignments;

[Collection("Assignments controller tests")]
public class CreateAssignmentsControllerTests(DoItApiFactory apiFactory)
    : IAsyncLifetime
{
    private readonly HttpClient _client = apiFactory.HttpClient;
    private readonly Func<Task> _resetDatabase = apiFactory.ResetDatabaseAsync;
    private readonly IAssignmentsRepository  _assignmentsRepository = apiFactory.Services.GetRequiredService<IAssignmentsRepository>();
    
    [Fact]
    public async Task Create_WhenInvokedWithProperData_ShouldSaveInDatabase()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "api/assignments",
            new CreateAssignmentRequest(
                Constants.Tasks.Title.Value,
                IsImportant: null,
                AssignmentsListId: null
            )
        );

        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        responseContent
            .Should()
            .BeEmpty();

        response.Headers.Location
            .Should()
            .NotBeNull();

        var assignmentsInDatabase = await _assignmentsRepository.GetAll();
        assignmentsInDatabase
            .Should()
            .HaveCount(1);
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}