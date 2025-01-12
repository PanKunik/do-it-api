using DoIt.Api.Domain.Tasks;
using DoIt.Api.TestUtils;
using FluentAssertions;

namespace DoIt.Api.Unit.Tests.Domain.Tasks;

public class TaskIdTests
{
    [Fact]
    public async System.Threading.Tasks.Task CreateFrom_WhenCalled_ShouldReturnExpectedObject()
    {
        // Arrange
        var createTaskId = () => TaskId.CreateFrom(Constants.Tasks.TaskId.Value);

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
    public async System.Threading.Tasks.Task CreateFrom_WhenPassedEmptyGuid_ShouldThrowException()
    {
        // Arrange
        var guid = Guid.Empty;
        var createTaskId = () => TaskId.CreateFrom(guid);

        // Act & Assert
        createTaskId
            .Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("Value cannot be empty. (Parameter 'value')")
            .WithParameterName("value");

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var left = TaskId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value);
        var right = TaskId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value);

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
        var left = TaskId.CreateFrom(Constants.Tasks.TaskIdFromIndex(0).Value);
        var right = TaskId.CreateFrom(Constants.Tasks.TaskIdFromIndex(1).Value);

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await System.Threading.Tasks.Task.CompletedTask;
    }
}
