using System.Net;
using System.Reflection;
using DoIt.Api.Controllers.TaskLists;
using DoIt.Api.Services.TaskLists;
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
    public async Task GetAll_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        _taskListsService
            .GetAll()
            .Returns(
                new List<TaskListDTO>
                {
                    new(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt
                    )
                }
            );
        
        // Act
        var result = await _cut.GetAll();
        
        // Assert
        result
            .Should()
            .NotBeNull();
        
        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetAll_OnSuccess_ShouldReturn200OkStatusCode()
    {
        // Arrange
        _taskListsService
            .GetAll()
            .Returns(
                new List<TaskListDTO>
                {
                    new(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt
                    )
                }
            );
        
        // Act
        var result = (OkObjectResult)await _cut.GetAll();
        
        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_OnSuccess_ShouldReturnListOfTaskListDTO()
    {
        // Arrange
        _taskListsService
            .GetAll()
            .Returns(new List<TaskListDTO>
                {
                    new(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt
                    )
                }
            );

        // Act
        var result = (OkObjectResult)await _cut.GetAll();

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(
                new List<TaskListDTO>
                {
                    new(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt
                    )
                }    
            );
    }

    [Fact]
    public async Task GetAll_WhenInvoked_ShouldCallTaskListsServiceGetAllOnce()
    {
        // Arrange
        _taskListsService
            .GetAll()
            .Returns(
                new List<TaskListDTO>
                {
                    new(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt
                    )
                }
            );
        
        // Act
        await _cut.GetAll();
        
        // Assert
        await _taskListsService
            .Received(1)
            .GetAll();
    }

    [Fact]
    public async Task GetAll_ShouldContainHttpAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(TaskListsController).GetMethod(nameof(TaskListsController.GetAll));
        
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
                new TaskListDTO(
                    Constants.TaskLists.TaskListId.Value,
                    Constants.TaskLists.Name.Value,
                    Constants.TaskLists.CreatedAt
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
                new TaskListDTO(
                    Constants.TaskLists.TaskListId.Value,
                    Constants.TaskLists.Name.Value,
                    Constants.TaskLists.CreatedAt
                )
            );

        // Act
        var result = (CreatedAtActionResult) await _cut.Create(request);

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_WhenInvoked_ShouldCallTaskListsServiceCreateOnceWithExpectedParameters()
    {
        // Arrange
        var request = new CreateTaskListRequest(Constants.TaskLists.Name.Value);
        
        _taskListsService
            .Create(request)
            .Returns(
                Result<TaskListDTO>.Success(
                    new TaskListDTO(
                        Constants.TaskLists.TaskListId.Value,
                        Constants.TaskLists.Name.Value,
                        Constants.TaskLists.CreatedAt
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
        var methodData = typeof(TaskListsController).GetMethod(nameof(TaskListsController.Create));

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
}