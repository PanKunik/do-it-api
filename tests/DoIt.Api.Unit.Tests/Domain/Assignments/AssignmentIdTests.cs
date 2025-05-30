using DoIt.Api.Domain;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;

namespace DoIt.Api.Unit.Tests.Domain.Assignments;

public class AssignmentIdTests
{
    [Fact]
    public async Task AssignmentIdCreateFrom_WhenCalledWithProperValue_ShouldReturnExpectedObjectResultWithValue()
    {
        // Arrange
        var createAssignmentId = AssignmentId.CreateFrom;

        // Act
        var createAssignmentIdResult = createAssignmentId(Constants.Tasks.AssignmentId.Value);

        // Assert
        createAssignmentIdResult
            .Should()
            .Match<Result<AssignmentId>>(r => r.IsSuccess);

        createAssignmentIdResult.Value!
            .Should()
            .Match<AssignmentId>(id => id.Value == Constants.Tasks.AssignmentId.Value);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task CreateFrom_WhenPassedEmptyGuid_ShouldReturnResultWithErrorAssignmentIdCannotBeEmpty()
    {
        // Arrange
        var createAssignmentId = AssignmentId.CreateFrom;

        // Act
        var createAssignmentIdResult = createAssignmentId(Guid.Empty);

        // Assert
        createAssignmentIdResult
            .Should()
            .Match<Result<AssignmentId>>(
                e => e.IsFailure
                  && e.Error == Errors.Assignment.IdCannotBeEmpty
            );

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var left = AssignmentId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value).Value!;
        var right = AssignmentId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value).Value!;

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
        var left = AssignmentId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value).Value!;
        var right = AssignmentId.CreateFrom(Constants.Tasks.TaskIdFromIndex(1).Value).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }
}