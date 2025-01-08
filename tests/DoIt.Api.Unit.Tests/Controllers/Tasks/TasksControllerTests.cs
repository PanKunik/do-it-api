using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Persistence.Repositories.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Net;

namespace DoIt.Api.Unit.Tests.Controllers.Tasks;

public class TasksControllerTests
{
    private readonly TasksController _cut;
    public TasksControllerTests()
    {
        var tasksRepository = Substitute.For<ITasksRepository>();
        tasksRepository.GetAll().Returns(new List<TaskDTO>());

        _cut = new TasksController(tasksRepository);
    }

    [Fact]
    public async Task Get_WhenInvoked_ShouldReturnOkObjectResult()
    {
        // Act
        var result = await _cut.Get();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_WhenInvoked_ShouldReturn200OKStatusCode()
    {
        // Act
        var result = (OkObjectResult)(await _cut.Get());

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnListOfTasks()
    {
        // Act
        var result = (OkObjectResult)(await _cut.Get());

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeOfType<List<GetTaskResponse>>();
    }
}
