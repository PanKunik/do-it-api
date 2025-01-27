using DoIt.Api.Domain.Tasks;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;
using FluentAssertions;

namespace DoIt.Api.Unit.Tests.Domain.Tasks;

public class TaskIdTests
{
    [Fact]
    public async System.Threading.Tasks.Task CreateFrom_WhenCalled_ShouldReturnExpectedObject()
    {
        // Arrange
        var createTaskId = () => TaskId.CreateFrom(Constants.Tasks.TaskId.Value).Value!;

        // Act
        var result = createTaskId();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<TaskId>(id => id.Value == Constants.Tasks.TaskId.Value);

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateFrom_WhenPassedEmptyGuid_ShouldReturnErrorResult()
    {
        // Arrange
        var guid = Guid.Empty;
        var createTaskId = () => TaskId.CreateFrom(guid);

        // Act
        var result = createTaskId();

        // Assert
        result
            .Should()
            .Match<Result<TaskId>>(
                e => e.IsSuccess == false
                  && e.Error == Errors.Task.EmptyTaskId
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
