using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;

namespace DoIt.Api.Unit.Tests.Domain.TaskLists;

public class TaskListIdTests
{
    [Fact]
    public async Task TaskListIdCreateFrom_WhenCalledWithProperValue_ShouldReturnExpectedObjectResultWithValue()
    {
        // Arrange
        var createTaskListId = TaskListId.CreateFrom;

        // Act
        var createTaskListIdResult = createTaskListId(Constants.TaskLists.TaskListId.Value);

        // Assert
        createTaskListIdResult
            .Should()
            .Match<Result<TaskListId>>(r => r.IsSuccess);

        createTaskListIdResult.Value!
            .Should()
            .Match<TaskListId>(id => id.Value == Constants.TaskLists.TaskListId.Value);
        
        await Task.CompletedTask;
    }

    [Fact]
    public async Task TaskListIdCreateFrom_WhenPassedEmptyGuid_ShouldReturnResultWithErrorTaskListIdCannotBeEmpty()
    {
        // Arrange
        var createTaskListId = TaskListId.CreateFrom;
        
        // Act
        var createTaskListIdResult = createTaskListId(Guid.Empty);
        
        // Assert
        createTaskListIdResult
            .Should()
            .Match<Result<TaskListId>>(
                e => e.IsFailure
                  && e.Error == Errors.TaskList.IdCannotBeEmpty
            );
        
        await Task.CompletedTask;
    }

    [Fact]
    public async Task Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var left = TaskListId.CreateFrom(Constants.TaskLists.TaskListIdFromIndex(0).Value).Value!;
        var right = TaskListId.CreateFrom(Constants.TaskLists.TaskListIdFromIndex(0).Value).Value!;
        
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
        var left = TaskListId.CreateFrom(Constants.TaskLists.TaskListIdFromIndex(0).Value).Value!;
        var right = TaskListId.CreateFrom(Constants.TaskLists.TaskListIdFromIndex(1).Value).Value!;
        
        // Act
        var result = left.Equals(right);
        
        // Assert
        result
            .Should()
            .BeFalse();
        
        await Task.CompletedTask;
    }
}