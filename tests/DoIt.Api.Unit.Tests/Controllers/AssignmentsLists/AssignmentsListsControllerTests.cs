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
    
    #region Delete

    [Fact]
    public async Task Delete_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        _assignmentsListsService
            .Delete(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Result.Success());

        // Act
        var result = await _cut.Delete(Constants.AssignmentsLists.AssignmentsListId.Value);

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
        _assignmentsListsService
            .Delete(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Result.Success());

        // Act
        var result = (NoContentResult) await _cut.Delete(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ShouldContainHttpDeleteAttributeWithExpectedTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsListsController).GetMethod("Delete");

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
    public async Task Delete_WhenInvoked_ShouldCallAssignmentsListsServiceDeleteOnceWithExpectedValue()
    {
        // Arrange
        _assignmentsListsService
            .Delete(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Result.Success());

        // Act
        var result = await _cut.Delete(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        await _assignmentsListsService
            .Received(1)
            .Delete(Arg.Is<Guid>(r => r == Constants.AssignmentsLists.AssignmentsListId.Value));
    }

    [Fact]
    public async Task Delete_WhenAssignmentsListNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _assignmentsListsService
            .Delete(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Errors.AssignmentsList.NotFound);

        // Act
        var result = await _cut.Delete(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task Delete_WhenAssignmentsListNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _assignmentsListsService
            .Delete(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Errors.AssignmentsList.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Delete(Constants.AssignmentsLists.AssignmentsListId.Value)).Value;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Delete_WhenAssignmentsListNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _assignmentsListsService
            .Delete(Constants.AssignmentsLists.AssignmentsListId.Value)
            .Returns(Errors.AssignmentsList.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Delete(Constants.AssignmentsLists.AssignmentsListId.Value)).Value as ProblemDetails;

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Status
            .Should()
            .Be((int) HttpStatusCode.NotFound);
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
    
    #region AttachAssignment
    
    [Fact]
    public async Task AttachAssignment_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        _assignmentsListsService
            .AttachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Result.Success());
        
        // Act
        var result = await _cut.AttachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
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
    public async Task AttachAssignment_OnSuccess_ShouldReturn204NoContentStatusCode()
    {
        // Arrange
        _assignmentsListsService
            .AttachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Result.Success());
        
        // Act
        var result = (NoContentResult) await _cut.AttachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
        );
        
        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AttachAssignment_ShouldContainHttpPutAttributeWithExpectedTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsListsController).GetMethod("AttachAssignment");
        
        // Assert
        var attribute = methodData!.GetCustomAttribute<HttpPutAttribute>();
        
        attribute
            .Should()
            .NotBeNull();
        
        attribute!.Template
            .Should()
            .BeEquivalentTo("{id:guid}/attach/{assignmentId:guid}");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task AttachAssignment_WhenInvoked_ShouldCallAssignmentsServiceAttachAssignmentOnceWithExpectedArguments()
    {
        // Arrange
        _assignmentsListsService
            .AttachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Result.Success());
        
        // Act
        await _cut.AttachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
        );
        
        // Assert
        await _assignmentsListsService
            .Received(1)
            .AttachAssignment(
                Arg.Is(Constants.AssignmentsLists.AssignmentsListId.Value),
                Arg.Is(Constants.Tasks.AssignmentId.Value)
            );
    }

    [Fact]
    public async Task AttachAssignment_WhenAssignmentNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _assignmentsListsService
            .AttachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = await _cut.AttachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
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
    public async Task AttachAssignment_WhenAssignmentNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _assignmentsListsService
            .AttachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = ((ObjectResult)await _cut.AttachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
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
    public async Task AttachAssignment_WhenAssignmentNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _assignmentsListsService
            .AttachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = ((ObjectResult)await _cut.AttachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
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
    
    #region DetachAssignment
    
    [Fact]
    public async Task DetachAssignment_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        _assignmentsListsService
            .DetachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Result.Success());
        
        // Act
        var result = await _cut.DetachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
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
    public async Task DetachAssignment_OnSuccess_ShouldReturn204NoContentStatusCode()
    {
        // Arrange
        _assignmentsListsService
            .DetachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Result.Success());
        
        // Act
        var result = (NoContentResult) await _cut.DetachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
        );
        
        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DetachAssignment_ShouldContainHttpPutAttributeWithExpectedTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsListsController).GetMethod("DetachAssignment");
        
        // Assert
        var attribute = methodData!.GetCustomAttribute<HttpPutAttribute>();
        
        attribute
            .Should()
            .NotBeNull();
        
        attribute!.Template
            .Should()
            .BeEquivalentTo("{id:guid}/detach/{assignmentId:guid}");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task DetachAssignment_WhenInvoked_ShouldCallAssignmentsServiceDetachAssignmentOnceWithExpectedArguments()
    {
        // Arrange
        _assignmentsListsService
            .DetachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Result.Success());
        
        // Act
        await _cut.DetachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
        );
        
        // Assert
        await _assignmentsListsService
            .Received(1)
            .DetachAssignment(
                Arg.Is(Constants.AssignmentsLists.AssignmentsListId.Value),
                Arg.Is(Constants.Tasks.AssignmentId.Value)
            );
    }

    [Fact]
    public async Task DetachAssignment_WhenAssignmentNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _assignmentsListsService
            .DetachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = await _cut.DetachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
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
    public async Task DetachAssignment_WhenAssignmentNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _assignmentsListsService
            .DetachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = ((ObjectResult)await _cut.DetachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
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
    public async Task DetachAssignment_WhenAssignmentNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _assignmentsListsService
            .DetachAssignment(
                Constants.AssignmentsLists.AssignmentsListId.Value,
                Constants.Tasks.AssignmentId.Value
            )
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = ((ObjectResult)await _cut.DetachAssignment(
            Constants.AssignmentsLists.AssignmentsListId.Value,
            Constants.Tasks.AssignmentId.Value
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