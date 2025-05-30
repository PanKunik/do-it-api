using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;
using DoIt.Api.TestUtils.Builders;

namespace DoIt.Api.Unit.Tests.Domain.Assignments;

public class AssignmentTests
{
    [Fact]
    public async Task AssignmentCreate_WhenPassedProperData_ShouldCreateExpectedObjectResultWithValue()
    {
        // Arrange
        var createAssignment = Assignment.Create;

        // Act
        var createAssignmentResult = createAssignment(
            Constants.Tasks.AssignmentId,
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.Done,
            Constants.Tasks.Important,
            Constants.AssignmentsLists.AssignmentsListId
        );

        // Assert
        createAssignmentResult
            .Should()
            .Match<Result<Assignment>>(r => r.IsSuccess);

        createAssignmentResult.Value!
            .Should()
            .Match<Assignment>(
                t => t.Id == Constants.Tasks.AssignmentId
                  && t.Title == Constants.Tasks.Title
                  && t.CreatedAt == Constants.Tasks.CreatedAt
                  && t.IsDone == Constants.Tasks.Done
                  && t.IsImportant == Constants.Tasks.Important
                  && t.AssignmentsListId! == Constants.AssignmentsLists.AssignmentsListId
            );

        await Task.CompletedTask;
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task ChangeState_WhenCalled_ShouldSwitchIsDoneFlag(
        bool actualState, bool desiredState
    )
    {
        // Arrange
        var cut = Assignment.Create(
            Constants.Tasks.AssignmentId,
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            isDone: actualState,
            Constants.Tasks.NotImportant,
            Constants.AssignmentsLists.AssignmentsListId
        ).Value!;

        // Act
        cut.ChangeState();

        // Assert
        cut.IsDone
            .Should()
            .Be(desiredState);

        await Task.CompletedTask;
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task ChangeImportance_WhenCalled_ShouldSwitchIsImportantFlag(
        bool actualState, bool desiredState    
    )
    {
        // Arrange
        var cut = Assignment.Create(
            Constants.Tasks.AssignmentId,
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            isImportant: actualState,
            Constants.AssignmentsLists.AssignmentsListId
        ).Value!;

        // Act
        cut.ChangeImportance();

        // Assert
        cut.IsImportant
            .Should()
            .Be(desiredState);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Equals_WhenCalledForObjectWithOtherValuesButSameAssignmentId_ShouldReturnTrue()
    {
        // Arrange
        var left = Assignment.Create(
            Constants.Tasks.AssignmentId,
            Constants.Tasks.TitleFromIndex(0),
            Constants.Tasks.CreatedAtFromIndex(0),
            Constants.Tasks.NotDone,
            Constants.Tasks.NotImportant,
            assignmentsListId: null
        ).Value!;

        var right = Assignment.Create(
            Constants.Tasks.AssignmentId,
            Constants.Tasks.TitleFromIndex(1),
            Constants.Tasks.CreatedAtFromIndex(1),
            Constants.Tasks.Done,
            Constants.Tasks.Important,
            Constants.AssignmentsLists.AssignmentsListId
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
    public async Task Equals_WhenCalledForObjectWithSameValuesButOtherAssignmentId_ShouldReturnFalse()
    {
        // Arrange
        var left = Assignment.Create(
            Constants.Tasks.TaskIdFromIndex(0),
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            Constants.Tasks.NotImportant,
            Constants.AssignmentsLists.AssignmentsListId
        ).Value!;

        var right = Assignment.Create(
            Constants.Tasks.TaskIdFromIndex(1),
            Constants.Tasks.Title,
            Constants.Tasks.CreatedAt,
            Constants.Tasks.NotDone,
            Constants.Tasks.NotImportant,
            Constants.AssignmentsLists.AssignmentsListId
        ).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task UpdateTile_WhenPassedProperData_ShouldUpdateTitle()
    {
        // Arrange
        var cut = Assignment.Create(
            id: Constants.Tasks.AssignmentId,
            title: Constants.Tasks.Title,
            createdAt: Constants.Tasks.CreatedAt,
            isDone: Constants.Tasks.NotDone,
            isImportant: Constants.Tasks.Important,
            assignmentsListId: null
        ).Value!;

        // Act
        cut.UpdateTitle(Title.CreateFrom("Updated task title").Value!);

        // Arrange
        cut.Title
            .Should()
            .BeEquivalentTo(Title.CreateFrom("Updated task title").Value!);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task AttachToList_WhenInvokedForAssignmentNotAttachedToAnyList_ShouldAttachToListWithPassedId()
    {
        // Arrange
        var assignment = AssignmentBuilder.Default().Build();
        var assignmentsListId = AssignmentsListBuilder.Default().Build().Id;
        
        // Act
        assignment.AttachToList(assignmentsListId);
        
        // Assert
        assignment.AssignmentsListId
            .Should()
            .NotBeNull();
        
        assignment.AssignmentsListId
            .Should()
            .Be(assignmentsListId);
        
        await Task.CompletedTask;
    }

    [Fact]
    public async Task AttachToList_WhenInvokedForAssignmentAlreadyAttachedToOtherList_ShouldChangeAttachedAttachmentListIdToPassedId()
    {
        // Arrange
        var assignmentsListId = AssignmentsListBuilder.Default().Build().Id;
        var assignment = AssignmentBuilder.Default().WithAssignmentsListId(assignmentsListId.Value).Build();
        
        var otherListId = AssignmentsListBuilder.Default(2).Build().Id;
        
        // Act
        assignment.AttachToList(otherListId);
        
        // Assert
        assignment.AssignmentsListId
            .Should()
            .NotBeNull();
        
        assignment.AssignmentsListId
            .Should()
            .Be(otherListId);
        
        await Task.CompletedTask;
    }

    [Fact]
    public async Task DetachFromList_WhenInvokedForAttachedAssignment_ShouldSetAssignmentsListIdToNull()
    {
        // Arrange
        var assignmentsListId = AssignmentsListBuilder.Default().Build().Id;
        var assignment = AssignmentBuilder.Default().WithAssignmentsListId(assignmentsListId.Value).Build();
        
        // Act
        assignment.DetachFromList();
        
        // Assert
        assignment.AssignmentsListId
            .Should()
            .BeNull();
        
        await Task.CompletedTask;
    }

    [Fact]
    public async Task DetachFromList_WhenInvokedForNotAttachedAssignments_ShouldLeaveAssignmentsListIdAsNull()
    {
        // Arrange
        var assignment = AssignmentBuilder.Default().Build();
        
        // Act
        assignment.DetachFromList();
        
        // Assert
        assignment.AssignmentsListId
            .Should()
            .BeNull();
        
        await Task.CompletedTask;
    }
}