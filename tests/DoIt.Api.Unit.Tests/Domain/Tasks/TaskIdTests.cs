using DoIt.Api.Domain.Tasks;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;

namespace DoIt.Api.Unit.Tests.Domain.Tasks;

public class TaskIdTests
{
    [Fact]
    public async System.Threading.Tasks.Task TaskIdCreateFrom_WhenCalledWithProperValue_ShouldReturnExpectedObjectResultWithValue()
    {
        // Arrange
        var createTaskId = () => TaskId.CreateFrom(Constants.Tasks.TaskId.Value);

        // Act
        var createTaskIdResult = createTaskId();

        // Assert
        createTaskIdResult
            .Should()
            .Match<Result<TaskId>>(r => r.IsSuccess);

        createTaskIdResult.Value!
            .Should()
            .Match<TaskId>(id => id.Value == Constants.Tasks.TaskId.Value);

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateFrom_WhenPassedEmptyGuid_ShouldReturnResultWithErrorTaskIdCannotBeEmpty()
    {
        // Arrange
        var createTaskId = () => TaskId.CreateFrom(Guid.Empty);

        // Act
        var createTaskIdResult = createTaskId();

        // Assert
        createTaskIdResult
            .Should()
            .Match<Result<TaskId>>(
                e => e.IsFailure
                  && e.Error == Errors.Task.IdCannotBeEmpty
            );

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var left = TaskId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value).Value!;
        var right = TaskId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeTrue();

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var left = TaskId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value).Value!;
        var right = TaskId.CreateFrom(Constants.Tasks.TaskIdFromIndex(1).Value).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await System.Threading.Tasks.Task.CompletedTask;
    }
}
