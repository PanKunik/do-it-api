using System.Net;
using System.Reflection;
using DoIt.Api.Controllers.Assignments;
using DoIt.Api.Domain;
using DoIt.Api.Services.Assignments;
using DoIt.Api.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Constants = DoIt.Api.TestUtils.Constants;

namespace DoIt.Api.Unit.Tests.Controllers.Assignments;

public class AssignmentsControllerTests
{
    private readonly IAssignmentsService _assignmentsService = Substitute.For<IAssignmentsService>();
    private readonly AssignmentsController _cut;

    public AssignmentsControllerTests()
    {
        _cut = new AssignmentsController(_assignmentsService);
        _cut.ControllerContext.HttpContext = new DefaultHttpContext();
    }

    [Fact]
    public async Task AssignmentsController_ShouldContainRouteAttributeWithExpectedTemplate()
    {
        // Arrange
        var attribute = typeof(AssignmentsController).GetCustomAttribute<RouteAttribute>();

        // Assert
        attribute
            .Should()
            .NotBeNull();

        attribute!.Template
            .Should()
            .BeEquivalentTo("api/assignments");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task AssignmentsController_ShouldContainApiControllerAttribute()
    {
        // Arrange
        var attribute = typeof(AssignmentsController).GetCustomAttribute<ApiControllerAttribute>();

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
        _assignmentsService
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
        _assignmentsService
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
    public async Task Get_OnSuccess_ShouldReturnListOfAssignmentDTO()
    {
        // Arrange
        _assignmentsService
            .GetAll()
            .Returns(
                [
                    new AssignmentDto(
                        Constants.Tasks.AssignmentId.Value,
                        Constants.Tasks.Title.Value,
                        Constants.Tasks.CreatedAt,
                        Constants.Tasks.NotDone,
                        Constants.Tasks.Important,
                        null
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
                new List<AssignmentDto>()
                {
                    new AssignmentDto(
                        Constants.Tasks.AssignmentId.Value,
                        Constants.Tasks.Title.Value,
                        Constants.Tasks.CreatedAt,
                        Constants.Tasks.NotDone,
                        Constants.Tasks.Important,
                        null
                    )
                }
            );
    }

    [Fact]
    public async Task Get_WhenInvoked_ShouldCallAssignmentsRepositoryGetAllOnce()
    {
        // Arrange
        _assignmentsService
            .GetAll()
            .Returns([]);

        // Act
        var result = await _cut.Get();

        // Assert
        await _assignmentsService
            .Received(1)
            .GetAll();
    }

    [Fact]
    public async Task Get_ShouldContainHttpGetAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsController).GetMethod("Get");

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
    public async Task GetById_WhenAssignmentNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _assignmentsService
            .GetById(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = await _cut.GetById(Constants.Tasks.AssignmentId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task GetById_WhenAssignmentNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _assignmentsService
            .GetById(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = (ObjectResult) await _cut.GetById(Constants.Tasks.AssignmentId.Value);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task GetById_WhenAssignmentNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _assignmentsService
            .GetById(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.GetById(Constants.Tasks.AssignmentId.Value)).Value as ProblemDetails;

        // Assert
        result!.Status
            .Should()
            .Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        _assignmentsService
            .GetById(Constants.Tasks.AssignmentId.Value)
            .Returns(
                new AssignmentDto(
                    Constants.Tasks.AssignmentId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.Done,
                    Constants.Tasks.NotImportant,
                    null
                )
            );

        // Act
        var result = await _cut.GetById(Constants.Tasks.AssignmentId.Value);

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
        _assignmentsService
            .GetById(Constants.Tasks.AssignmentId.Value)
            .Returns(
                Result<AssignmentDto>.Success(
                    new AssignmentDto(
                        Constants.Tasks.AssignmentId.Value,
                        Constants.Tasks.Title.Value,
                        Constants.Tasks.CreatedAt,
                        Constants.Tasks.Done,
                        Constants.Tasks.NotImportant,
                        null
                    )
                )
            );

        // Act
        var result = (OkObjectResult) await _cut.GetById(Constants.Tasks.AssignmentId.Value);

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_OnSuccess_ShouldReturnExpectedAssignmentDTO()
    {
        // Arrange
        _assignmentsService
            .GetById(Constants.Tasks.AssignmentId.Value)
            .Returns(
                new AssignmentDto(
                    Constants.Tasks.AssignmentId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.Done,
                    Constants.Tasks.NotImportant,
                    AssignmentsListId: null
                )
            );

        // Act
        var result = (OkObjectResult) await _cut.GetById(Constants.Tasks.AssignmentId.Value);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Match<AssignmentDto>(
                r => r.Id == Constants.Tasks.AssignmentId.Value
                  && r.Title == Constants.Tasks.Title.Value
                  && r.CreatedAt == Constants.Tasks.CreatedAt
                  && r.IsDone == Constants.Tasks.Done
                  && r.IsImportant == Constants.Tasks.NotImportant
            );
    }

    [Fact]
    public async Task GetById_WhenInvoked_ShouldCallAssignmentsServiceGetByIdOnceWithExpectedArgument()
    {
        // Arrange
        _assignmentsService
            .GetById(Constants.Tasks.AssignmentId.Value)
            .Returns(
                new AssignmentDto(
                    Constants.Tasks.AssignmentId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.Done,
                    Constants.Tasks.NotImportant,
                    AssignmentsListId: null
                )
            );

        // Act
        await _cut.GetById(Constants.Tasks.AssignmentId.Value);

        // Assert
        await _assignmentsService
            .Received(1)
            .GetById(Arg.Is<Guid>(a => a == Constants.Tasks.AssignmentId.Value));
    }

    [Fact]
    public async Task GetById_ShouldContainHttpGetAttributeWithExpectedTemplate()
    {
        // Arrange
        var methodData = typeof(AssignmentsController).GetMethod("GetById");
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
        var request = new CreateAssignmentRequest(
            Constants.Tasks.Title.Value,
            IsImportant: null,
            AssignmentsListId: null
        );
        
        _assignmentsService
            .Create(request)
            .Returns(
                new AssignmentDto(
                    Constants.Tasks.AssignmentId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.NotDone,
                    Constants.Tasks.NotImportant,
                    AssignmentsListId: null
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
            .Be(nameof(AssignmentsController.GetById));

        actionResult.RouteValues
            .Should()
            .ContainEquivalentOf(
                new KeyValuePair<string, Guid>(
                    "id",
                    Constants.Tasks.AssignmentId.Value
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
        var request = new CreateAssignmentRequest(
            Constants.Tasks.Title.Value,
            IsImportant: null,
            AssignmentsListId: null
        );
        
        _assignmentsService
            .Create(request)
            .Returns(
                new AssignmentDto(
                    Constants.Tasks.AssignmentId.Value,
                    Constants.Tasks.Title.Value,
                    Constants.Tasks.CreatedAt,
                    Constants.Tasks.NotDone,
                    Constants.Tasks.NotImportant,
                    AssignmentsListId: null
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
    public async Task Create_WhenInvoked_ShouldCallAssignmentsRepositoryCreateOnceWithExpectedParameter()
    {
        // Arrange
        var request = new CreateAssignmentRequest(
            Constants.Tasks.Title.Value,
            Constants.Tasks.Important,
            Constants.AssignmentsLists.AssignmentsListId.Value
        );
        
        _assignmentsService
            .Create(request)
            .Returns(
                Result<AssignmentDto>.Success(
                    new AssignmentDto(
                        Constants.Tasks.AssignmentId.Value,
                        Constants.Tasks.Title.Value,
                        Constants.Tasks.CreatedAt,
                        Constants.Tasks.NotDone,
                        Constants.Tasks.Important,
                        Constants.AssignmentsLists.AssignmentsListId.Value
                    )
                )
            );

        // Act
        await _cut.Create(request);

        // Assert
        await _assignmentsService
            .Received(1)
            .Create(Arg.Is<CreateAssignmentRequest>(r => r.Title == Constants.Tasks.Title.Value));
    }
    
    [Fact]
    public async Task Create_ShouldContainHttpPostAttributeWithoutTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsController).GetMethod("Create");

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
        _assignmentsService
            .Delete(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());

        // Act
        var result = await _cut.Delete(Constants.Tasks.AssignmentId.Value);

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
        _assignmentsService
            .Delete(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());

        // Act
        var result = (NoContentResult) await _cut.Delete(Constants.Tasks.AssignmentId.Value);

        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ShouldContainHttpDeleteAttributeWithExpectedTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsController).GetMethod("Delete");

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
    public async Task Delete_WhenInvoked_ShouldCallAssignmentsServiceDeleteOnceWithExpectedValue()
    {
        // Arrange
        _assignmentsService
            .Delete(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());

        // Act
        var result = await _cut.Delete(Constants.Tasks.AssignmentId.Value);

        // Assert
        await _assignmentsService
            .Received(1)
            .Delete(Arg.Is<Guid>(r => r == Constants.Tasks.AssignmentId.Value));
    }

    [Fact]
    public async Task Delete_WhenAssignmentNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _assignmentsService
            .Delete(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = await _cut.Delete(Constants.Tasks.AssignmentId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task Delete_WhenAssignmentNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _assignmentsService
            .Delete(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Delete(Constants.Tasks.AssignmentId.Value)).Value;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Delete_WhenAssignmentNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _assignmentsService
            .Delete(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Delete(Constants.Tasks.AssignmentId.Value)).Value as ProblemDetails;

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
        var request = new UpdateAssignmentRequest(Constants.Tasks.Title.Value);
        _assignmentsService
            .Update(
                Constants.Tasks.AssignmentId.Value,
                request
            )
            .Returns(Result.Success());

        // Act
        var result = await _cut.Update(
            Constants.Tasks.AssignmentId.Value,
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
        var request = new UpdateAssignmentRequest(Constants.Tasks.Title.Value);
        _assignmentsService
            .Update(
                Constants.Tasks.AssignmentId.Value,
                request
            )
            .Returns(Result.Success());

        // Act
        var result = (NoContentResult) await _cut.Update(
            Constants.Tasks.AssignmentId.Value,
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
        var methodData = typeof(AssignmentsController).GetMethod("Update");

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
    public async Task Update_WhenInvoked_ShouldCallAssignmentsServiceUpdateOnceWithExpectedArguments()
    {
        // Arrange
        var request = new UpdateAssignmentRequest(Constants.Tasks.Title.Value);
        _assignmentsService
            .Update(
                Constants.Tasks.AssignmentId.Value,
                request
            )
            .Returns(Result.Success());

        // Act
        await _cut.Update(
            Constants.Tasks.AssignmentId.Value,
            request
        );

        // Assert
        await _assignmentsService
            .Received(1)
            .Update(
                Arg.Is(Constants.Tasks.AssignmentId.Value),
                Arg.Is(request)
            );
    }

    [Fact]
    public async Task Update_WhenAssignmentNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        var request = new UpdateAssignmentRequest(Constants.Tasks.Title.Value);
        _assignmentsService
            .Update(
                Constants.Tasks.AssignmentId.Value,
                request
            )
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = await _cut.Update(
            Constants.Tasks.AssignmentId.Value,
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
    public async Task Update_WhenAssignmentNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        var request = new UpdateAssignmentRequest(Constants.Tasks.Title.Value);
        _assignmentsService
            .Update(
                Constants.Tasks.AssignmentId.Value,
                request
            )
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Update(
            Constants.Tasks.AssignmentId.Value,
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
    public async Task Update_WhenAssignmentNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        var request = new UpdateAssignmentRequest(Constants.Tasks.Title.Value);
        _assignmentsService
            .Update(
                Constants.Tasks.AssignmentId.Value,
                request
            )
            .Returns(Errors.Assignment.NotFound);

        // Act
        var result = ((ObjectResult) await _cut.Update(
            Constants.Tasks.AssignmentId.Value,
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
    
    #region ChangeState

    [Fact]
    public async Task ChangeState_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        _assignmentsService
            .ChangeState(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());
        
        // Act
        var result = await _cut.ChangeState(Constants.Tasks.AssignmentId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();
        
        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task ChangeState_OnSuccess_ShouldReturn204NoContentStatusCode()
    {
        // Arrange
        _assignmentsService
            .ChangeState(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());
        
        // Act
        var result = (NoContentResult) await _cut.ChangeState(Constants.Tasks.AssignmentId.Value);
        
        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ChangeState_ShouldContainHttpPutAttributeWithExpectedTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsController).GetMethod("ChangeState");
        
        // Assert
        var attribute = methodData!.GetCustomAttribute<HttpPutAttribute>();
        
        attribute
            .Should()
            .NotBeNull();
        
        attribute!.Template
            .Should()
            .BeEquivalentTo("{id:guid}/change-state");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ChangeState_WhenInvoked_ShouldCallAssignmentsServiceChangeStateOnceWithExpectedArguments()
    {
        // Arrange
        _assignmentsService
            .ChangeState(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());
        
        // Act
        await _cut.ChangeState(Constants.Tasks.AssignmentId.Value);
        
        // Assert
        await _assignmentsService
            .Received(1)
            .ChangeState(Arg.Is(Constants.Tasks.AssignmentId.Value));
    }

    [Fact]
    public async Task ChangeState_WhenAssignmentNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _assignmentsService
            .ChangeState(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = await _cut.ChangeState(Constants.Tasks.AssignmentId.Value);
        
        // Assert
        result
            .Should()
            .NotBeNull();
        
        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task ChangeState_WhenAssignmentNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _assignmentsService
            .ChangeState(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = ((ObjectResult)await _cut.ChangeState(Constants.Tasks.AssignmentId.Value)).Value;
        
        // Assert
        result
            .Should()
            .NotBeNull();
        
        result
            .Should()
            .BeOfType<ProblemDetails>();
    }
    
    [Fact]
    public async Task ChangeState_WhenAssignmentNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _assignmentsService
            .ChangeState(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = ((ObjectResult)await _cut.ChangeState(Constants.Tasks.AssignmentId.Value)).Value as ProblemDetails;
        
        // Assert
        result
            .Should()
            .NotBeNull();
        
        result!.Status
            .Should()
            .Be((int) HttpStatusCode.NotFound);
    }
    
    #endregion
    
    #region ChangeImportance

    [Fact]
    public async Task ChangeImportance_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        _assignmentsService
            .ChangeImportance(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());
        
        // Act
        var result = await _cut.ChangeImportance(Constants.Tasks.AssignmentId.Value);

        // Assert
        result
            .Should()
            .NotBeNull();
        
        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task ChangeImportance_OnSuccess_ShouldReturn204NoContentStatusCode()
    {
        // Arrange
        _assignmentsService
            .ChangeImportance(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());
        
        // Act
        var result = (NoContentResult) await _cut.ChangeImportance(Constants.Tasks.AssignmentId.Value);
        
        // Assert
        result.StatusCode
            .Should()
            .Be((int) HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ChangeImportance_ShouldContainHttpPutAttributeWithExpectedTemplate()
    {
        // Act
        var methodData = typeof(AssignmentsController).GetMethod("ChangeImportance");
        
        // Assert
        var attribute = methodData!.GetCustomAttribute<HttpPutAttribute>();
        
        attribute
            .Should()
            .NotBeNull();
        
        attribute!.Template
            .Should()
            .BeEquivalentTo("{id:guid}/change-importance");

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ChangeImportance_WhenInvoked_ShouldCallAssignmentsServiceChangeImportanceOnceWithExpectedArguments()
    {
        // Arrange
        _assignmentsService
            .ChangeImportance(Constants.Tasks.AssignmentId.Value)
            .Returns(Result.Success());
        
        // Act
        await _cut.ChangeImportance(Constants.Tasks.AssignmentId.Value);
        
        // Assert
        await _assignmentsService
            .Received(1)
            .ChangeImportance(Arg.Is(Constants.Tasks.AssignmentId.Value));
    }

    [Fact]
    public async Task ChangeImportance_WhenAssignmentNotFound_ShouldReturnObjectResult()
    {
        // Arrange
        _assignmentsService
            .ChangeImportance(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = await _cut.ChangeImportance(Constants.Tasks.AssignmentId.Value);
        
        // Assert
        result
            .Should()
            .NotBeNull();
        
        result
            .Should()
            .BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task ChangeImportance_WhenAssignmentNotFound_ShouldReturnProblemDetailsAsValue()
    {
        // Arrange
        _assignmentsService
            .ChangeImportance(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = ((ObjectResult)await _cut.ChangeImportance(Constants.Tasks.AssignmentId.Value)).Value;
        
        // Assert
        result
            .Should()
            .NotBeNull();
        
        result
            .Should()
            .BeOfType<ProblemDetails>();
    }
    
    [Fact]
    public async Task ChangeImportance_WhenAssignmentNotFound_ShouldReturn404NotFoundStatusCode()
    {
        // Arrange
        _assignmentsService
            .ChangeImportance(Constants.Tasks.AssignmentId.Value)
            .Returns(Errors.Assignment.NotFound);
        
        // Act
        var result = ((ObjectResult)await _cut.ChangeImportance(Constants.Tasks.AssignmentId.Value)).Value as ProblemDetails;
        
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