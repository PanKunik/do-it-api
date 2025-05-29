using System.Net;
using System.Reflection;
using DoIt.Api.Controllers.TaskLists;
using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Services.TaskLists;
using DoIt.Api.Services.Tasks;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DoIt.Api.Unit.Tests.Controllers.TaskLists;

public class TaskListsControllerTests
{
    private readonly ITaskListsService _taskListsService = Substitute.For<ITaskListsService>();
    private readonly TaskListsController _cut;

    public TaskListsControllerTests()
    {
        _cut = new TaskListsController(_taskListsService);
        _cut.ControllerContext.HttpContext = new DefaultHttpContext();
    }

    [Fact]
    public async Task TaskListsController_ShouldContainRouteAttributeWithExpectedTemplate()
    {
        // Arrange
        var attribute = typeof(TaskListsController).GetCustomAttribute<RouteAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeEquivalentTo("api/task-lists");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task TaskListsController_ShouldContainApiControllerAttribute()
    {
        // Arrange
        var attribute = typeof(TaskListsController).GetCustomAttribute<ApiControllerAttribute>();

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
        _taskListsService
            .GetAll()
            .Returns([]);

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
        _taskListsService
            .GetAll()
            .Returns([]);

        // Act
        var result = (OkObjectResult) await _cut.Get();

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_OnSuccess_ShouldReturnListOfTaskListDTO()
    {
        // Arrange
        _taskListsService
            .GetAll()
            .Returns(
                [
                    new TaskListDto(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt,
                        Tasks: null
                    )
                ]
            );

        // Act
        var result = (OkObjectResult) await _cut.Get();

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeEquivalentTo(
                new List<TaskListDto>()
                {
                    new(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt,
                        Tasks: null
                    )
                }
            );
    }
    
    [Fact]
    public async Task Get_WhenInvoked_ShouldCallTaskListsRepositoryGetAllOnce()
    {
        // Arrange
        _taskListsService
            .GetAll()
            .Returns([]);

        // Act
        var result = await _cut.Get();

        // Assert
        await _taskListsService
            .Received(1)
            .GetAll();
    }
    
    [Fact]
    public async Task Get_ShouldContainHttpGetAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(TaskListsController).GetMethod("Get");

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
    
    #region Create

    [Fact]
    public async Task Create_OnSuccess_ShouldReturnExpectedCreatedAtActionObject()
    {
        // Arrange
        var request = new CreateTaskListRequest(Constants.TaskLists.Name.Value);

        _taskListsService
            .Create(request)
            .Returns(
                new TaskListDto(
                    Constants.TaskLists.TaskListId.Value,
                    Constants.TaskLists.Name.Value,
                    Constants.TaskLists.CreatedAt,
                    TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
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
            .Be(nameof(TaskListsController.GetById));

        actionResult!.RouteValues
            .Should()
            .ContainEquivalentOf(
                new KeyValuePair<string, Guid>(
                    "id",
                    Constants.TaskLists.TaskListId.Value
                )
            );

        actionResult!.Value
            .Should()
            .BeNull();
    }

    [Fact]
    public async Task Create_OnSuccess_ShouldReturn201CreatedStatusCode()
    {
        // Arrange
        var request = new CreateTaskListRequest(Constants.TaskLists.Name.Value);

        _taskListsService
            .Create(request)
            .Returns(
                new TaskListDto(
                    Constants.TaskLists.TaskListId.Value,
                    Constants.TaskLists.Name.Value,
                    Constants.TaskLists.CreatedAt,
                    TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                )
            );

        // Act
        var result = (CreatedAtActionResult)await _cut.Create(request);

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public async Task
        Create_WhenInvoked_ShouldCallTaskListsServiceCreateOnceWithExpectedParameters()
    {
        // Arrange
        var request = new CreateTaskListRequest(Constants.TaskLists.Name.Value);

        _taskListsService
            .Create(request)
            .Returns(
                Result<TaskListDto>.Success(
                    new TaskListDto(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt,
                        TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                    )
                )
            );

        // Act
        var result = await _cut.Create(request);

        // Assert
        await _taskListsService
            .Received(1)
            .Create(Arg.Is<CreateTaskListRequest>(r => r.Name == Constants.TaskLists.Name.Value));
    }

    [Fact]
    public async Task Create_ShouldContainHttpPostAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(TaskListsController).GetMethod("Create");

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

    #region GetById

    [Fact]
    public async Task GetById_WhenTaskListNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _taskListsService
            .GetById(Constants.TaskLists.TaskListId.Value)
            .Returns(Errors.TaskList.NotFound);

        // Act
        var result = await _cut.GetById(Constants.TaskLists.TaskListId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task GetById_WhenTaskListNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _taskListsService
            .GetById(Constants.TaskLists.TaskListId.Value)
            .Returns(Errors.TaskList.NotFound);

        // Act
        var result = (ObjectResult)await _cut.GetById(Constants.TaskLists.TaskListId.Value);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task GetById_WhenTaskListNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _taskListsService
            .GetById(Constants.TaskLists.TaskListId.Value)
            .Returns(Errors.TaskList.NotFound);

        // Act
        var result = ((ObjectResult)await _cut.GetById(Constants.TaskLists.TaskListId.Value)).Value as ProblemDetails;

        // Assert
        result!.Status
            .Should()
            .Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        _taskListsService
            .GetById(Constants.TaskLists.TaskListId.Value)
            .Returns(
                new TaskListDto(
                    Constants.TaskLists.TaskListId.Value,
                    Constants.TaskLists.Name.Value,
                    Constants.TaskLists.CreatedAt,
                    TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                )
            );

        // Act
        var result = await _cut.GetById(Constants.TaskLists.TaskListId.Value);

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
        _taskListsService
            .GetById(Constants.TaskLists.TaskListId.Value)
            .Returns(
                Result<TaskListDto>.Success(
                    new TaskListDto(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt,
                        TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                    )
                )
            );

        // Act
        var result = (OkObjectResult)await _cut.GetById(Constants.TaskLists.TaskListId.Value);

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnExpectedTaskDTO()
    {
        // Arrange
        _taskListsService
            .GetById(Constants.TaskLists.TaskListId.Value)
            .Returns(
                new TaskListDto(
                    Constants.TaskLists.TaskListId.Value,
                    Constants.TaskLists.Name.Value,
                    Constants.TaskLists.CreatedAt,
                    TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                )
            );

        // Act
        var result = (OkObjectResult)await _cut.GetById(Constants.TaskLists.TaskListId.Value);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Match<TaskListDto>(
                r => r.Id == Constants.TaskLists.TaskListId.Value
                     && r.Name == Constants.TaskLists.Name.Value
                     && r.CreatedAt == Constants.TaskLists.CreatedAt
            );
    }

    [Fact]
    public async Task
        GetById_WhenInvoked_ShouldCallTaskListsServiceGetByIdOnceWithExpectedArgument()
    {
        // Arrange
        _taskListsService
            .GetById(Constants.TaskLists.TaskListId.Value)
            .Returns(
                new TaskListDto(
                    Constants.TaskLists.TaskListId.Value,
                    Constants.TaskLists.Name.Value,
                    Constants.TaskLists.CreatedAt,
                    TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                )
            );

        // Act
        await _cut.GetById(Constants.TaskLists.TaskListId.Value);

        // Assert
        await _taskListsService
            .Received(1)
            .GetById(Arg.Is<Guid>(a => a == Constants.TaskLists.TaskListId.Value));
    }

    [Fact]
    public async Task GetById_ShouldContainHttpGetAttributeWithExpectedTemplate()
    {
        // Arrange
        var methodData = typeof(TaskListsController).GetMethod("GetById");
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
}