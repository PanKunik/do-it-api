using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Unit.Tests.Domain.TaskLists;

public class TaskListTests
{
    [Fact]
    public async System.Threading.Tasks.Task TaskListCreate_WhenPassedProperData_ShouldCreateExpectedObjectResultWithValue()
    {
        // Arrange
        var createTaskList = TaskList.Create;
        var tasks = TaskListsUtilities.CreateTasks(3);
        
        // Act
        var createTaskListResult = createTaskList(
            Constants.TaskLists.TaskListId,
            Constants.TaskLists.Name,
            Constants.TaskLists.CreatedAt,
            tasks
        );

        // Assert
        createTaskListResult
            .Should()
            .Match<Result<TaskList>>(r => r.IsSuccess);

        createTaskListResult.Value!
            .Should()
            .Match<TaskList>(
                l => l.Id == Constants.TaskLists.TaskListId
                  && l.Name == Constants.TaskLists.Name
                  && l.CreatedAt == Constants.TaskLists.CreatedAt
                  && l.Tasks.SequenceEqual(tasks)
            );
        
        await System.Threading.Tasks.Task.CompletedTask;
    }
    [Fact]
    public async System.Threading.Tasks.Task TaskListCreate_WhenPassedNullAsTasks_ShouldCreateExpectedObjectResultWithValue()
    {
        // Arrange
        var createTaskList = TaskList.Create;
        
        // Act
        var createTaskListResult = createTaskList(
            Constants.TaskLists.TaskListId,
            Constants.TaskLists.Name,
            Constants.TaskLists.CreatedAt,
            null
        );

        // Assert
        createTaskListResult
            .Should()
            .Match<Result<TaskList>>(r => r.IsSuccess);

        createTaskListResult.Value!
            .Should()
            .Match<TaskList>(
                l => l.Id == Constants.TaskLists.TaskListId
                     && l.Name == Constants.TaskLists.Name
                     && l.CreatedAt == Constants.TaskLists.CreatedAt
                     && l.Tasks.SequenceEqual(new List<Task>())
            );
        
        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task TaskListCreate_WhenPassedNullTaskListId_ShouldReturnResultWithErrorTaskListIdCannotBeNull()
    {
        // Arrange
        var createTaskList = TaskList.Create;

        // Act
        var createTaskListResult = createTaskList(
            null!,   // taskListId
            Constants.TaskLists.Name,
            Constants.TaskLists.CreatedAt,
            TaskListsUtilities.CreateTasks(3)
        );

        // Assert
        createTaskListResult
            .Should()
            .Match<Result<TaskList>>(
                e => e.IsFailure
                  && e.Error == Errors.TaskList.IdCannotBeNull
            );
        
        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task TaskListCreate_WhenPassedNullName_ShouldReturnResultWithErrorTaskListNameCannotBeNull()
    {
        // Arrange
        var createTaskList = TaskList.Create;
        
        // Act
        var createTaskListResult = createTaskList(
            Constants.TaskLists.TaskListId,
            null!,  // name
            Constants.TaskLists.CreatedAt,
            TaskListsUtilities.CreateTasks(3)
        );
        
        // Assert
        createTaskListResult
            .Should()
            .Match<Result<TaskList>>(
                r => r.IsFailure
                  && r.Error == Errors.TaskList.NameCannotBeNull
            );
        
        await System.Threading.Tasks.Task.CompletedTask;
    }
    
    [Fact]
    public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithOtherValuesButSameTaskListId_ShouldReturnTrue()
    {
        // Arrange
        var left = TaskList.Create(
            Constants.TaskLists.TaskListId,
            Constants.TaskLists.NameFromIndex(0),
            Constants.TaskLists.CreatedAtFromIndex(0),
            TaskListsUtilities.CreateTasks(2)
        ).Value!;

        var right = TaskList.Create(
            Constants.TaskLists.TaskListId,
            Constants.TaskLists.NameFromIndex(1),
            Constants.TaskLists.CreatedAtFromIndex(1),
            TaskListsUtilities.CreateTasks(3)
        ).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeTrue();

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithSameValuesButOtherTaskIListd_ShouldReturnFalse()
    {
        // Arrange
        var left = TaskList.Create(
            Constants.TaskLists.TaskListIdFromIndex(0),
            Constants.TaskLists.NameFromIndex(0),
            Constants.TaskLists.CreatedAtFromIndex(0),
            TaskListsUtilities.CreateTasks(3)
        ).Value!;

        var right = TaskList.Create(
            Constants.TaskLists.TaskListIdFromIndex(1),
            Constants.TaskLists.NameFromIndex(0),
            Constants.TaskLists.CreatedAtFromIndex(0),
            TaskListsUtilities.CreateTasks(3)
        ).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await System.Threading.Tasks.Task.CompletedTask;
    }
}