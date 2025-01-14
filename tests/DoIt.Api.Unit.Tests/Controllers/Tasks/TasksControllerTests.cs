using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Services.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using System.Net;
using System.Reflection;

namespace DoIt.Api.Unit.Tests.Controllers.Tasks;

public class TasksControllerTests
{
    private readonly ITasksService _tasksService = Substitute.For<ITasksService>();
    private readonly TasksController _cut;

    public TasksControllerTests()
    {
        _cut = new TasksController(_tasksService);
    }

    [Fact]
    public async Task TaskController_ShouldContainRouteAttributeWithExpectedTemplate()
    {
        // Arrange
        var attribute = typeof(TasksController).GetCustomAttribute<RouteAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeEquivalentTo("api/[controller]");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task TaskController_ShouldContainApiControllerAttribute()
    {
        // Arrange
        var attribute = typeof(TasksController).GetCustomAttribute<ApiControllerAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        await Task.CompletedTask;
    }

    #region GetAll

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        _tasksService
            .GetAll()
            .Returns(new List<GetTaskResponse>());

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
    public async Task Get_OnSuccess_ShouldReturn200OKStatusCode()
    {
        // Arrange
        _tasksService
            .GetAll()
            .Returns(new List<GetTaskResponse>());

        // Act
        var result = (OkObjectResult) await _cut.Get();

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnListOfGetTaskResponse()
    {
        // Arrange
        _tasksService
            .GetAll()
            .Returns(new List<GetTaskResponse>());

        // Act
        var result = (OkObjectResult) await _cut.Get();

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeOfType<List<GetTaskResponse>>();
    }

    [Fact]
    public async Task Get_WhenInvoked_ShouldCallTasksRepositoryGetAllOnce()
    {
        // Arrange
        _tasksService
            .GetAll()
            .Returns(new List<GetTaskResponse>());

        // Act
        var result = await _cut.Get();

        // Assert
        await _tasksService
            .Received(1)
            .GetAll();
    }

    [Fact]
    public async Task Get_ShouldContainHttpGetAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(TasksController).GetMethod("Get");

        // Assert
        var attribute = methodData!.GetCustomAttribute<HttpGetAttribute>();

        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeNull();

        await Task.CompletedTask;
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_WhenTaskNotFound_ShouldReturnNotFoundResult()
    {
        // Act
        var result = await _cut.GetById(Guid.NewGuid());

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetById_WhenTaskNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Act
        var result = (NotFoundResult) await _cut.GetById(Guid.NewGuid());

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _tasksService
            .GetById(Arg.Is<Guid>(r => r == guid))
            .Returns(
                new GetTaskResponse(
                    guid,
                    "Test title 1",
                    DateTime.UtcNow,
                    false,
                    false
                )
            );

        // Act
        var result = await _cut.GetById(guid);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturn200OKStatusCode()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _tasksService
            .GetById(Arg.Is<Guid>(r => r == guid))
            .Returns(
                new GetTaskResponse(
                    guid,
                    "Test title 1",
                    DateTime.UtcNow,
                    false,
                    false
                )
            );

        // Act
        var result = (OkObjectResult) await _cut.GetById(guid);

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnGetTaskResponse()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var creatdAt = DateTime.UtcNow;
        _tasksService
            .GetById(Arg.Is<Guid>(a => a == guid))
            .Returns(
                new GetTaskResponse(
                    guid,
                    "Test title 1",
                    creatdAt,
                    false,
                    false
                )
            );

        // Act
        var result = (OkObjectResult) await _cut.GetById(guid);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Match<GetTaskResponse>(
                r => r.Id == guid
                  && r.Title == "Test title 1"
                  && r.CreatedAt == creatdAt
                  && r.IsDone == false
                  && r.IsImportant == false
            );
    }

    [Fact]
    public async Task GetById_WhenInvoked_ShouldCallTasksServiceGetByIdOnceWithExpectedArgument()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _tasksService
            .GetById(Arg.Is<Guid>(r => r == guid))
            .Returns(
                new GetTaskResponse(
                    guid,
                    "Test title 1",
                    DateTime.UtcNow,
                    false,
                    false
                )
            );

        // Act
        await _cut.GetById(guid);

        // Assert
        await _tasksService
            .Received(1)
            .GetById(Arg.Is<Guid>(a => a == guid));
    }

    [Fact]
    public async Task GetById_ShouldContainHttpGetAttributeWithExpectedTemplate()
    {
        // Arrange
        var methodData = typeof(TasksController).GetMethod("GetById");
        var attribute = methodData!.GetCustomAttribute<HttpGetAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeEquivalentTo("{id:guid}");

        await Task.CompletedTask;
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_OnSuccess_ShouldReturnCreatedAtActionObjectWithExpectedValues()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var request = new CreateTaskRequest("Task title 1");
        _tasksService
            .Create(Arg.Any<CreateTaskRequest>())
            .Returns(
                new CreateTaskResponse(
                    guid,
                    "Task title 1",
                    DateTime.UtcNow,
                    false,
                    false
                )
            );

        // Act
        var result = await _cut.Create(request);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<CreatedAtActionResult>();

        var actionResult = result as CreatedAtActionResult;
        actionResult!.ActionName
            .Should()
            .Be(nameof(TasksController.GetById));

        actionResult!.RouteValues
            .Should()
            .ContainEquivalentOf(
                new KeyValuePair<string, string>(
                    "id",
                    guid.ToString("N")
                )
            );

        actionResult!.Value
            .Should()
            .Match<CreateTaskResponse>(r => r.Title == "Task title 1"); // TODO: Assert the rest of the object
    }

    [Fact]
    public async Task Create_OnSuccess_ShouldReturn201CreatedStatusCode()
    {
        // Arrange
        var request = new CreateTaskRequest("Task title 1");
        _tasksService
            .Create(Arg.Any<CreateTaskRequest>())
            .Returns(
                new CreateTaskResponse(
                    Guid.NewGuid(),
                    "Test title 1",
                    DateTime.UtcNow,
                    false,
                    false
                )
            );

        // Act
        var result = (CreatedAtActionResult) await _cut.Create(request);

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_WhenInvoked_ShouldCallTasksRepositoryCreateOnceWithExpectedParameter()
    {
        // Arrange
        var title = string.Empty;
        _tasksService
            .Create(Arg.Do<CreateTaskRequest>(p => title = p.Title))
            .Returns(
                new CreateTaskResponse(
                    Guid.NewGuid(),
                    title,
                    DateTime.UtcNow,
                    false,
                    false
                )
            );

        // Act
        var result = await _cut.Create(new CreateTaskRequest("Test title 1"));

        // Assert
        await _tasksService
            .Received(1)
            .Create(Arg.Is<CreateTaskRequest>(r => r.Title == "Test title 1")); // TODO: Assert rest of object
    }
    
    [Fact]
    public async Task Create_ShouldContainHttpPostAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(TasksController).GetMethod("Create");

        // Assert
        var attribute = methodData!.GetCustomAttribute<HttpPostAttribute>();

        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeNull();

        await Task.CompletedTask;
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        _tasksService
            .Delete(Arg.Any<Guid>())
            .Returns(true);

        // Act
        var result = await _cut.Delete(Guid.NewGuid());

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_OnSuccess_ShouldReturn201NoContentStatusCode()
    {
        // Arrange
        _tasksService
            .Delete(Arg.Any<Guid>())
            .Returns(true);

        // Act
        var result = (NoContentResult) await _cut.Delete(Guid.NewGuid());

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ShouldContainHttpDeleteAttributeWithExpectedTemplate()
    {
        // Act
        var methodData = typeof(TasksController).GetMethod("Delete");

        // Assert
        var attribute = methodData!.GetCustomAttribute<HttpDeleteAttribute>();

        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeEquivalentTo("{id:guid}");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Delete_WhenInvoked_ShouldCallTasksServiceDeleteOnceWithExpectedValue()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = await _cut.Delete(guid);

        // Assert
        await _tasksService
            .Received(1)
            .Delete(Arg.Is<Guid>(r => r == guid));
    }

    #endregion
}
