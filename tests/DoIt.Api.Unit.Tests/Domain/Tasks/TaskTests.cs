using DoIt.Api.Domain.Tasks;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Unit.Tests.Domain.Tasks;

public class TaskTests
{
    [Fact]
    public async System.Threading.Tasks.Task TaskCreate_WhenPassedProperData_ShouldCreateExpectedObjectResultWithValue()
    {
        // Arrange
        var createTask = Task.Create;

        // Act
        var createTaskResult = createTask(
            Constants.Tasks.TaskId,
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.Done,
            Constants.Tasks.Important,
            Constants.TaskLists.TaskListId
        );

        // Assert
        createTaskResult
            .Should()
            .Match<Result<Task>>(r => r.IsSuccess);

        createTaskResult.Value!
            .Should()
            .Match<Task>(
                t => t.Id == Constants.Tasks.TaskId
                  && t.Title == Constants.Tasks.Title
                  && t.CreatedAt == Constants.Tasks.CreatedAt
                  && t.IsDone == Constants.Tasks.Done
                  && t.IsImportant == Constants.Tasks.Important
                  && t.TaskListId! == Constants.TaskLists.TaskListId
            );

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task TaskCreate_WhenPassedNullTaskId_ShouldReturnResultWithErrorTaskIdCannotBeNull()
    {
        // Arrange
        var createTask = Task.Create;

        // Act
        var createTaskResult = createTask(
            null!, // taskId
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            Constants.Tasks.NotImportant,
            Constants.TaskLists.TaskListId
        );

        // Assert
        createTaskResult
            .Should()
            .Match<Result<Task>>(
                e => e.IsFailure
                  && e.Error == Errors.Task.IdCannotBeNull
            );

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTask_WhenPassedNullTitle_ShouldReturnResultWithErrorTaskTitleCannotBeNull()
    {
        // Arrange
        var createTask = Task.Create;

        // Act
        var createTaskResult = createTask(
            Constants.Tasks.TaskId,
            null!, // title
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            Constants.Tasks.Important,
            Constants.TaskLists.TaskListId
        );

        // Assert
        createTaskResult
            .Should()
            .Match<Result<Task>>(
                e => e.IsSuccess == false
                  && e.Error == Errors.Task.TitleCannotBeNull
            );

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async System.Threading.Tasks.Task ChangeState_WhenCalled_ShouldSwitchIsDoneFlag(
        bool actualState, bool desiredState
    )
    {
        // Arrange
        var cut = Task.Create(
            Constants.Tasks.TaskId,
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            isDone: actualState,
            Constants.Tasks.NotImportant,
            Constants.TaskLists.TaskListId
        ).Value!;

        // Act
        cut.ChangeState();

        // Assert
        cut.IsDone
            .Should()
            .Be(desiredState);

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async System.Threading.Tasks.Task ChangeImportance_WhenCalled_ShouldSwitchIsImportantFlag(
        bool actualState, bool desiredState    
    )
    {
        // Arrange
        var cut = Task.Create(
            Constants.Tasks.TaskId,
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            isImportant: actualState,
            Constants.TaskLists.TaskListId
        ).Value!;

        // Act
        cut.ChangeImportance();

        // Assert
        cut.IsImportant
            .Should()
            .Be(desiredState);

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithOtherValuesButSameTaskId_ShouldReturnTrue()
    {
        // Arrange
        var left = Task.Create(
            Constants.Tasks.TaskId,
            Constants.Tasks.TitleFromIndex(0),
            Constants.Tasks.CreatedAtFromIndex(0),
            Constants.Tasks.NotDone,
            Constants.Tasks.NotImportant,
            taskListId: null
        ).Value!;

        var right = Task.Create(
            Constants.Tasks.TaskId,
            Constants.Tasks.TitleFromIndex(1),
            Constants.Tasks.CreatedAtFromIndex(1),
            Constants.Tasks.Done,
            Constants.Tasks.Important,
            Constants.TaskLists.TaskListId
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
    public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithSameValuesButOtherTaskId_ShouldReturnFalse()
    {
        // Arrange
        var left = Task.Create(
            Constants.Tasks.TaskIdFromIndex(0),
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            Constants.Tasks.NotImportant,
            Constants.TaskLists.TaskListId
        ).Value!;

        var right = Task.Create(
            Constants.Tasks.TaskIdFromIndex(1),
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            Constants.Tasks.NotImportant,
            Constants.TaskLists.TaskListId
        ).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTitle_WhenCalledWithNullTitle_ShouldReturnResultWithErrorTaskTitleCannotBeNull()
    {
        // Arrange
        var cut = Task.Create(
            Constants.Tasks.TaskId,
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            Constants.Tasks.Important,
            taskListId: null
        ).Value!;

        // Act
        var updateTitleResult = cut.UpdateTitle(null!);

        // Act & Assert
        updateTitleResult
            .Should()
            .Match<Result>(
                r => r.IsFailure
                  && r.Error == Errors.Task.TitleCannotBeNull
            );

        await System.Threading.Tasks.Task.CompletedTask;
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTile_WhenPassedProperData_ShouldUpdateTitle()
    {
        // Arrange
        var cut = Task.Create(
            taskId: Constants.Tasks.TaskId,
            title: Constants.Tasks.Title,
            createdAt: Constants.Tasks.CreatedAt,
            isDone: Constants.Tasks.NotDone,
            isImportant: Constants.Tasks.Important,
            taskListId: null
        ).Value!;

        // Act
        cut.UpdateTitle(Title.CreateFrom("Updated task title").Value!);

        // Arrange
        cut.Title
            .Should()
            .BeEquivalentTo(Title.CreateFrom("Updated task title").Value!);

        await System.Threading.Tasks.Task.CompletedTask;
    }
}