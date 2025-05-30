using DoIt.Api.Domain;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;

namespace DoIt.Api.Unit.Tests.Domain.AssignmentsLists;

public class AssignmentsListIdTests
{
    [Fact]
    public async Task AssignmentsListIdCreateFrom_WhenCalledWithProperValue_ShouldReturnExpectedObjectResultWithValue()
    {
        // Arrange
        var createAssignmentsListId = AssignmentsListId.CreateFrom;

        // Act
        var createAssginmentsListIdResult = createAssignmentsListId(Constants.AssignmentsLists.AssignmentsListId.Value);

        // Assert
        createAssginmentsListIdResult
            .Should()
            .Match<Result<AssignmentsListId>>(r => r.IsSuccess);

        createAssginmentsListIdResult.Value!
            .Should()
            .Match<AssignmentsListId>(id => id.Value == Constants.AssignmentsLists.AssignmentsListId.Value);
        
        await Task.CompletedTask;
    }

    [Fact]
    public async Task TaskListIdCreateFrom_WhenPassedEmptyGuid_ShouldReturnResultWithErrorTaskListIdCannotBeEmpty()
    {
        // Arrange
        var createAssignmentsListId = AssignmentsListId.CreateFrom;
        
        // Act
        var createAssignmentsListIdResult = createAssignmentsListId(Guid.Empty);
        
        // Assert
        createAssignmentsListIdResult
            .Should()
            .Match<Result<AssignmentsListId>>(
                e => e.IsFailure
                  && e.Error == Errors.AssignmentsList.IdCannotBeEmpty
            );
        
        await Task.CompletedTask;
    }

    [Fact]
    public async Task Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var left = AssignmentsListId.CreateFrom(Constants.AssignmentsLists.TaskListIdFromIndex(0).Value).Value!;
        var right = AssignmentsListId.CreateFrom(Constants.AssignmentsLists.TaskListIdFromIndex(0).Value).Value!;
        
        // Act
        var result = left.Equals(right);
        
        // Assert
        result
            .Should()
            .BeTrue();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Equals_WhenCalledForObjectWithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var left = AssignmentsListId.CreateFrom(Constants.AssignmentsLists.TaskListIdFromIndex(0).Value).Value!;
        var right = AssignmentsListId.CreateFrom(Constants.AssignmentsLists.TaskListIdFromIndex(1).Value).Value!;
        
        // Act
        var result = left.Equals(right);
        
        // Assert
        result
            .Should()
            .BeFalse();
        
        await Task.CompletedTask;
    }
}