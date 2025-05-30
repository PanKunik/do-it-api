using System.Net;
using System.Reflection;
using DoIt.Api.Controllers.AssignmentsLists;
using DoIt.Api.Domain;
using DoIt.Api.Services.AssignmentsLists;
using DoIt.Api.Services.Assignments;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DoIt.Api.Unit.Tests.Controllers.AssignmentsLists;

public class AssignmentsListsControllerTests
{
    private readonly IAssignmentsListsService _assignmentsListsService = Substitute.For<IAssignmentsListsService>();
    private readonly AssignmentsListsController _cut;

    public AssignmentsListsControllerTests()
    {
        _cut = new AssignmentsListsController(_assignmentsListsService);
        _cut.ControllerContext.HttpContext = new DefaultHttpContext();
    }

    [Fact]
    public async Task AssignmentsListsController_ShouldContainRouteAttributeWithExpectedTemplate()
    {
        // Arrange
        var attribute = typeof(AssignmentsListsController).GetCustomAttribute<RouteAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeEquivalentTo("api/assignments-lists");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task AssignmentsListsController_ShouldContainApiControllerAttribute()
    {
        // Arrange
        var attribute = typeof(AssignmentsListsController).GetCustomAttribute<ApiControllerAttribute>();

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
        _assignmentsListsService
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
        _assignmentsListsService
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
    public async Task Get_OnSuccess_ShouldReturnListOfAssignmentsListDTO()
    {
        // Arrange
        _assignmentsListsService
            .GetAll()
            .Returns(
                [
                    new AssignmentsListDto(
                        Constants.AssignmentsLists.AssignmentsListId.Value,
                        Constants.AssignmentsLists.Name.Value,
                        Constants.AssignmentsLists.CreatedAt,
                        Assignments: null
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
                new List<AssignmentsListDto>()
                {
                    new(
                        Constants.AssignmentsLists.AssignmentsListId.Value,
                        Constants.AssignmentsLists.Name.Value,
                        Constants.AssignmentsLists.CreatedAt,
                        Assignments: null
                    )
                }
            );
    }
    
    [Fact]
    public async Task Get_WhenInvoked_ShouldCallAssignmentsListsRepositoryGetAllOnce()
    {
        // Arrange
        _assignmentsListsService
            .GetAll()
            .Returns([]);

        // Act
        var result = await _cut.Get();

        // Assert
        await _assignmentsListsService
            .Received(1)
            .GetAll();
    }
    
    [Fact]
    public async Task Get_ShouldContainHttpGetAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsListsController).GetMethod("Get");

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
        var request = new CreateAssignmentsListRequest(Constants.AssignmentsLists.Name.Value);

        _assignmentsListsService
            .Create(request)
            .Returns(
                new AssignmentsListDto(
                    Constants.AssignmentsLists.AssignmentsListId.Value,
                    Constants.AssignmentsLists.Name.Value,
                    Constants.AssignmentsLists.CreatedAt,
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
            .Be(nameof(AssignmentsListsController.GetById));

        actionResult!.RouteValues
            .Should()
            .ContainEquivalentOf(
                new KeyValuePair<string, Guid>(
                    "id",
                    Constants.AssignmentsLists.AssignmentsListId.Value
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
        var request = new CreateAssignmentsListRequest(Constants.AssignmentsLists.Name.Value);

        _assignmentsListsService
            .Create(request)
            .Returns(
                new AssignmentsListDto(
                    Constants.AssignmentsLists.AssignmentsListId.Value,
                    Constants.AssignmentsLists.Name.Value,
                    Constants.AssignmentsLists.CreatedAt,
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
        Create_WhenInvoked_ShouldCallAssignmentsListsServiceCreateOnceWithExpectedParameters()
    {
        // Arrange
        var request = new CreateAssignmentsListRequest(Constants.AssignmentsLists.Name.Value);

        _assignmentsListsService
            .Create(request)
            .Returns(
                Result<AssignmentsListDto>.Success(
                    new AssignmentsListDto(
                        Constants.AssignmentsLists.AssignmentsListId.Value,
                        Constants.AssignmentsLists.Name.Value,
                        Constants.AssignmentsLists.CreatedAt,
                        TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                    )
                )
            );

        // Act
        var result = await _cut.Create(request);

        // Assert
        await _assignmentsListsService
            .Received(1)
            .Create(Arg.Is<CreateAssignmentsListRequest>(r => r.Name == Constants.AssignmentsLists.Name.Value));
    }

    [Fact]
    public async Task Create_ShouldContainHttpPostAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsListsController).GetMethod("Create");

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
    public async Task GetById_WhenAssignmentsListNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _assignmentsListsService
            .GetById(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Errors.AssignmentsList.NotFound);

        // Act
        var result = await _cut.GetById(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task GetById_WhenAssignmentsListNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _assignmentsListsService
            .GetById(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Errors.AssignmentsList.NotFound);

        // Act
        var result = (ObjectResult)await _cut.GetById(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task GetById_WhenAssignmentsListNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _assignmentsListsService
            .GetById(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Errors.AssignmentsList.NotFound);

        // Act
        var result = ((ObjectResult)await _cut.GetById(Constants.AssignmentsLists.AssignmentsListId.Value)).Value as ProblemDetails;

        // Assert
        result!.Status
            .Should()
            .Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        _assignmentsListsService
            .GetById(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(
                new AssignmentsListDto(
                    Constants.AssignmentsLists.AssignmentsListId.Value,
                    Constants.AssignmentsLists.Name.Value,
                    Constants.AssignmentsLists.CreatedAt,
                    TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                )
            );

        // Act
        var result = await _cut.GetById(Constants.AssignmentsLists.AssignmentsListId.Value);

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
        _assignmentsListsService
            .GetById(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(
                Result<AssignmentsListDto>.Success(
                    new AssignmentsListDto(
                        Constants.AssignmentsLists.AssignmentsListId.Value,
                        Constants.AssignmentsLists.Name.Value,
                        Constants.AssignmentsLists.CreatedAt,
                        TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                    )
                )
            );

        // Act
        var result = (OkObjectResult)await _cut.GetById(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        result.StatusCode
            .Should()
            .Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnExpectedTaskDTO()
    {
        // Arrange
        _assignmentsListsService
            .GetById(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(
                new AssignmentsListDto(
                    Constants.AssignmentsLists.AssignmentsListId.Value,
                    Constants.AssignmentsLists.Name.Value,
                    Constants.AssignmentsLists.CreatedAt,
                    TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                )
            );

        // Act
        var result = (OkObjectResult)await _cut.GetById(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Match<AssignmentsListDto>(
                r => r.Id == Constants.AssignmentsLists.AssignmentsListId.Value
                     && r.Name == Constants.AssignmentsLists.Name.Value
                     && r.CreatedAt == Constants.AssignmentsLists.CreatedAt
            );
    }

    [Fact]
    public async Task
        GetById_WhenInvoked_ShouldCallAssignmentsListsServiceGetByIdOnceWithExpectedArgument()
    {
        // Arrange
        _assignmentsListsService
            .GetById(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(
                new AssignmentsListDto(
                    Constants.AssignmentsLists.AssignmentsListId.Value,
                    Constants.AssignmentsLists.Name.Value,
                    Constants.AssignmentsLists.CreatedAt,
                    TaskListsUtilities.CreateTasks().Select(t => t.ToDto()).ToList()
                )
            );

        // Act
        await _cut.GetById(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        await _assignmentsListsService
            .Received(1)
            .GetById(Arg.Is<Guid>(a => a == Constants.AssignmentsLists.AssignmentsListId.Value));
    }

    [Fact]
    public async Task GetById_ShouldContainHttpGetAttributeWithExpectedTemplate()
    {
        // Arrange
        var methodData = typeof(AssignmentsListsController).GetMethod("GetById");
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