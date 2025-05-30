using DoIt.Api.Domain;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;
using DoIt.Api.TestUtils.Builders;

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
        var left = AssignmentsListBuilder.Default(2).WithName(Name.CreateFrom("List #1").Value!).Build();
        var right = AssignmentsListBuilder.Default(2).WithName(Name.CreateFrom("List #2").Value!).Build();

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
        var left = AssignmentsListBuilder.Default(2).Build();
        var right = AssignmentsListBuilder.Default(3).Build();

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task AttachAssignment_WhenInvoked_ShouldAttachAssignmentToAssignmentsList()
    {
        // Arrange
        var otherAssignment = AssignmentBuilder.Default(2).Build();
        var assignmentsList = AssignmentsListBuilder.Default().WithAssignment(otherAssignment).Build();
        var assignment = AssignmentBuilder.Default().Build();
        
        // Act
        assignmentsList.AttachAssignment(assignment);
        
        // Assert
        assignmentsList.Assignments
            .SequenceEqual([otherAssignment, assignment])
            .Should()
            .BeTrue();
        
        await Task.CompletedTask;
    }

    [Fact]
    public async Task DetachAssignment_WhenInvokedForAssignmentAttachedToList_ShouldDetachAssignmentFromList()
    {
        // Arrange
        var assignment = AssignmentBuilder.Default().Build();
        var otherAssignment = AssignmentBuilder.Default(2).Build();
        var assignmentsList = AssignmentsListBuilder.Default().WithAssignment(assignment).WithAssignment(otherAssignment).Build();
        
        // Act
        assignmentsList.DetachAssignment(assignment);
        
        // Assert
        assignmentsList.Assignments
            .SequenceEqual([otherAssignment])
            .Should()
            .BeTrue();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task DetachAssignment_WhenInvokedForNotExistingAssignment_ShouldReturnErrorResultAssignmentNotFound()
    {
        // Arrange
        var assignmentsList =  AssignmentsListBuilder.Default().Build();
        var assignment = AssignmentBuilder.Default().Build();
        
        // Act
        var result = assignmentsList.DetachAssignment(assignment);
        
        // Assert
        result
            .Should()
            .NotBeNull();

        result.Error
            .Should()
            .Be(Errors.Assignment.NotFound);
        
        await Task.CompletedTask;
    }
}