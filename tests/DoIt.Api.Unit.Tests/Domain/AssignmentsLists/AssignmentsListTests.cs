using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;

namespace DoIt.Api.Unit.Tests.Domain.AssignmentsLists;

public class AssignmentsListTests
{
    [Fact]
    public async Task AssignmentsListCreate_WhenPassedProperData_ShouldCreateExpectedObjectResultWithValue()
    {
        // Arrange
        var createAssignmentsList = AssignmentsList.Create;
        var assignments = TaskListsUtilities.CreateTasks(3);
        
        // Act
        var createAssignmentsListResult = createAssignmentsList(
            Constants.AssignmentsLists.AssignmentsListId,
            Constants.AssignmentsLists.Name,
            Constants.AssignmentsLists.CreatedAt,
            assignments
        );

        // Assert
        createAssignmentsListResult
            .Should()
            .Match<Result<AssignmentsList>>(r => r.IsSuccess);

        createAssignmentsListResult.Value!
            .Should()
            .Match<AssignmentsList>(
                l => l.Id == Constants.AssignmentsLists.AssignmentsListId
                  && l.Name == Constants.AssignmentsLists.Name
                  && l.CreatedAt == Constants.AssignmentsLists.CreatedAt
                  && l.Assignments.SequenceEqual(assignments)
            );
        
        await Task.CompletedTask;
    }
    [Fact]
    public async Task AssignmentsListCreate_WhenPassedNullAsAssignments_ShouldCreateExpectedObjectResultWithValue()
    {
        // Arrange
        var createAssignmentsList = AssignmentsList.Create;
        
        // Act
        var createAssignmentsListResult = createAssignmentsList(
            Constants.AssignmentsLists.AssignmentsListId,
            Constants.AssignmentsLists.Name,
            Constants.AssignmentsLists.CreatedAt,
            null
        );

        // Assert
        createAssignmentsListResult
            .Should()
            .Match<Result<AssignmentsList>>(r => r.IsSuccess);

        createAssignmentsListResult.Value!
            .Should()
            .Match<AssignmentsList>(
                l => l.Id == Constants.AssignmentsLists.AssignmentsListId
                     && l.Name == Constants.AssignmentsLists.Name
                     && l.CreatedAt == Constants.AssignmentsLists.CreatedAt
                     && l.Assignments.SequenceEqual(new List<Assignment>())
            );
        
        await Task.CompletedTask;
    }
    
    [Fact]
    public async Task Equals_WhenCalledForObjectWithOtherValuesButSameAssignmentsListId_ShouldReturnTrue()
    {
        // Arrange
        var left = AssignmentsList.Create(
            Constants.AssignmentsLists.AssignmentsListId,
            Constants.AssignmentsLists.NameFromIndex(0),
            Constants.AssignmentsLists.CreatedAtFromIndex(0),
            TaskListsUtilities.CreateTasks(2)
        ).Value!;

        var right = AssignmentsList.Create(
            Constants.AssignmentsLists.AssignmentsListId,
            Constants.AssignmentsLists.NameFromIndex(1),
            Constants.AssignmentsLists.CreatedAtFromIndex(1),
            TaskListsUtilities.CreateTasks(3)
        ).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeTrue();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Equals_WhenCalledForObjectWithSameValuesButOtherAssignmentsListId_ShouldReturnFalse()
    {
        // Arrange
        var left = AssignmentsList.Create(
            Constants.AssignmentsLists.TaskListIdFromIndex(0),
            Constants.AssignmentsLists.NameFromIndex(0),
            Constants.AssignmentsLists.CreatedAtFromIndex(0),
            TaskListsUtilities.CreateTasks(3)
        ).Value!;

        var right = AssignmentsList.Create(
            Constants.AssignmentsLists.TaskListIdFromIndex(1),
            Constants.AssignmentsLists.NameFromIndex(0),
            Constants.AssignmentsLists.CreatedAtFromIndex(0),
            TaskListsUtilities.CreateTasks(3)
        ).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }
}