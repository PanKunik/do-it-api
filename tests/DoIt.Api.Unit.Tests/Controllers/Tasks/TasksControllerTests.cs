using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Services.Tasks;
using DoIt.Api.Shared;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Net;
using System.Reflection;
using Task = System.Threading.Tasks.Task;
using Constants = DoIt.Api.TestUtils.Constants;
using Microsoft.AspNetCore.Http;
using DoIt.Api.Domain.Tasks;

namespace DoIt.Api.Unit.Tests.Controllers.Tasks;

public class TasksControllerTests
{
    private readonly ITasksService _tasksService = Substitute.For<ITasksService>();
    private readonly TasksController _cut;

    public TasksControllerTests()
    {
        _cut = new TasksController(_tasksService);
        _cut.ControllerContext.HttpContext = new DefaultHttpContext();
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
            .BeEquivalentTo("api/tasks");

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
            .Returns(new List<TaskDTO>());

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
            .Returns(new List<TaskDTO>());

        // Act
        var result = (OkObjectResult) await _cut.Get();

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnListOfTaskDTO()
    {
        // Arrange
        _tasksService
            .GetAll()
            .Returns(
                new List<TaskDTO>()
                {
                    new TaskDTO(
                        Constants.Tasks.TaskId.Value,
                        Constants.Tasks.Title.Value,
                        Constants.Tasks.CreatedAt,
                        Constants.Tasks.NotDone,
                        Constants.Tasks.Important
                    )
                }
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
                new List<TaskDTO>()
                {
                    new TaskDTO(
                        Constants.Tasks.TaskId.Value,
                        Constants.Tasks.Title.Value,
                        Constants.Tasks.CreatedAt,
                        Constants.Tasks.NotDone,
                        Constants.Tasks.Important
                    )
                }
            );
    }

    [Fact]
    public async Task Get_WhenInvoked_ShouldCallTasksRepositoryGetAllOnce()
    {
        // Arrange
        _tasksService
            .GetAll()
            .Returns(new List<TaskDTO>());

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
    public async Task GetById_WhenTaskNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _tasksService
            .GetById(Constants.Tasks.TaskId.Value)
            .Returns(Errors.Task.NotFound);

        // Act
        var result = await _cut.GetById(Constants.Tasks.TaskId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task GetById_WhenTaskNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _tasksService
            .GetById(Constants.Tasks.TaskId.Value)
            .Returns(Errors.Task.NotFound);

        // Act
        var result = (ObjectResult) await _cut.GetById(Constants.Tasks.TaskId.Value);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task GetById_WhenTaskNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _tasksService
            .GetById(Constants.Tasks.TaskId.Value)
            .Returns(Errors.Task.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.GetById(Constants.Tasks.TaskId.Value)).Value as ProblemDetails;

        // Assert
        result!.Status
            .Should()
            .Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        _tasksService
            .GetById(Constants.Tasks.TaskId.Value)
            .Returns(
                new TaskDTO(
                    Constants.Tasks.TaskId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.Done,
                    Constants.Tasks.NotImportant
                )
            );

        // Act
        var result = await _cut.GetById(Constants.Tasks.TaskId.Value);

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
        _tasksService
            .GetById(Constants.Tasks.TaskId.Value)
            .Returns(
                Result<TaskDTO>.Success(
                    new TaskDTO(
                        Constants.Tasks.TaskId.Value,
                        Constants.Tasks.Title.Value,
                        Constants.Tasks.CreatedAt,
                        Constants.Tasks.Done,
                        Constants.Tasks.NotImportant
                    )
                )
            );

        // Act
        var result = (OkObjectResult) await _cut.GetById(Constants.Tasks.TaskId.Value);

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnExpectedTaskDTO()
    {
        // Arrange
        _tasksService
            .GetById(Constants.Tasks.TaskId.Value)
            .Returns(
                new TaskDTO(
                    Constants.Tasks.TaskId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.Done,
                    Constants.Tasks.NotImportant
                )
            );

        // Act
        var result = (OkObjectResult) await _cut.GetById(Constants.Tasks.TaskId.Value);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Match<TaskDTO>(
                r => r.Id == Constants.Tasks.TaskId.Value
                  && r.Title == Constants.Tasks.Title.Value
                  && r.CreatedAt == Constants.Tasks.CreatedAt
                  && r.IsDone == Constants.Tasks.Done
                  && r.IsImportant == Constants.Tasks.NotImportant
            );
    }

    [Fact]
    public async Task GetById_WhenInvoked_ShouldCallTasksServiceGetByIdOnceWithExpectedArgument()
    {
        // Arrange
        _tasksService
            .GetById(Constants.Tasks.TaskId.Value)
            .Returns(
                new TaskDTO(
                    Constants.Tasks.TaskId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.Done,
                    Constants.Tasks.NotImportant
                )
            );

        // Act
        await _cut.GetById(Constants.Tasks.TaskId.Value);

        // Assert
        await _tasksService
            .Received(1)
            .GetById(Arg.Is<Guid>(a => a == Constants.Tasks.TaskId.Value));
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
    public async Task Create_OnSuccess_ShouldReturnExpectedCreatedAtActionObject()
    {
        // Arrange
        var request = new CreateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Create(request)
            .Returns(
                new TaskDTO(
                    Constants.Tasks.TaskId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.NotDone,
                    Constants.Tasks.NotImportant
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
                new KeyValuePair<string, Guid>(
                    "id",
                    Constants.Tasks.TaskId.Value
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
        var request = new CreateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Create(request)
            .Returns(
                new TaskDTO(
                    Constants.Tasks.TaskId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.NotDone,
                    Constants.Tasks.NotImportant
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
    public async Task Create_WhenInvoked_ShouldCallTasksRepositoryCreateOnceWithExpectedParameter()
    {
        // Arrange
        var request = new CreateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Create(request)
            .Returns(
                Result<TaskDTO>.Success(
                    new TaskDTO(
                        Constants.Tasks.TaskId.Value,
                        Constants.Tasks.Title.Value,
                        Constants.Tasks.CreatedAt,
                        Constants.Tasks.NotDone,
                        Constants.Tasks.NotImportant
                    )
                )
            );

        // Act
        var result = await _cut.Create(request);

        // Assert
        await _tasksService
            .Received(1)
            .Create(Arg.Is<CreateTaskRequest>(r => r.Title == Constants.Tasks.Title.Value));
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
            .Delete(Constants.Tasks.TaskId.Value)
            .Returns(Result.Success());

        // Act
        var result = await _cut.Delete(Constants.Tasks.TaskId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_OnSuccess_ShouldReturn204NoContentStatusCode()
    {
        // Arrange
        _tasksService
            .Delete(Constants.Tasks.TaskId.Value)
            .Returns(Result.Success());

        // Act
        var result = (NoContentResult) await _cut.Delete(Constants.Tasks.TaskId.Value);

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.NoContent);
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
        _tasksService
            .Delete(Constants.Tasks.TaskId.Value)
            .Returns(Result.Success());

        // Act
        var result = await _cut.Delete(Constants.Tasks.TaskId.Value);

        // Assert
        await _tasksService
            .Received(1)
            .Delete(Arg.Is<Guid>(r => r == Constants.Tasks.TaskId.Value));
    }

    [Fact]
    public async Task Delete_WhenTaskNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _tasksService
            .Delete(Constants.Tasks.TaskId.Value)
            .Returns(Errors.Task.NotFound);

        // Act
        var result = await _cut.Delete(Constants.Tasks.TaskId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task Delete_WhenTaskNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _tasksService
            .Delete(Constants.Tasks.TaskId.Value)
            .Returns(Errors.Task.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Delete(Constants.Tasks.TaskId.Value)).Value;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Delete_WhenTaskNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _tasksService
            .Delete(Constants.Tasks.TaskId.Value)
            .Returns(Errors.Task.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Delete(Constants.Tasks.TaskId.Value)).Value as ProblemDetails;

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Status
            .Should()
            .Be((int) HttpStatusCode.NotFound);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        var request = new UpdateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Update(
                Constants.Tasks.TaskId.Value,
                request
            )
            .Returns(Result.Success());

        // Act
        var result = await _cut.Update(
            Constants.Tasks.TaskId.Value,
            request
        );
        
        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_OnSuccess_ShouldReturn204NoContentStatusCode()
    {
        // Arrange
        var request = new UpdateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Update(
                Constants.Tasks.TaskId.Value,
                request
            )
            .Returns(Result.Success());

        // Act
        var result = (NoContentResult) await _cut.Update(
            Constants.Tasks.TaskId.Value,
            request
        );

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Update_ShouldContainHttpPutAttributeWithExpectedTemplate()
    {
        // Act
        var methodData = typeof(TasksController).GetMethod("Update");

        // Assert
        var attribute = methodData!.GetCustomAttribute<HttpPutAttribute>();

        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeEquivalentTo("{id:guid}");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Update_WhenInvoked_ShouldCallTasksServiceUpdateOnceWithExpectedArguments()
    {
        // Arrange
        var request = new UpdateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Update(
                Constants.Tasks.TaskId.Value,
                request
            )
            .Returns(Result.Success());

        // Act
        await _cut.Update(
            Constants.Tasks.TaskId.Value,
            request
        );

        // Assert
        await _tasksService
            .Received(1)
            .Update(
                Arg.Is(Constants.Tasks.TaskId.Value),
                Arg.Is(request)
            );
    }

    [Fact]
    public async Task Update_WhenTaskNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        var request = new UpdateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Update(
                Constants.Tasks.TaskId.Value,
                request
            )
            .Returns(Errors.Task.NotFound);

        // Act
        var result = await _cut.Update(
            Constants.Tasks.TaskId.Value,
            request
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task Update_WhenTaskNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        var request = new UpdateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Update(
                Constants.Tasks.TaskId.Value,
                request
            )
            .Returns(Errors.Task.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Update(
            Constants.Tasks.TaskId.Value,
            request
        )).Value;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Update_WhenTaskNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        var request = new UpdateTaskRequest(Constants.Tasks.Title.Value);
        _tasksService
            .Update(
                Constants.Tasks.TaskId.Value,
                request
            )
            .Returns(Errors.Task.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Update(
            Constants.Tasks.TaskId.Value,
            request
        )).Value as ProblemDetails;

        // Assert
        result
            .Should()
            .NotBeNull();
        
        result!.Status
            .Should()
            .Be((int) HttpStatusCode.NotFound);
    }

    #endregion
}
