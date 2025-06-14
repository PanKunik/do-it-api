using DoIt.Api.Domain;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;

namespace DoIt.Api.Unit.Tests.Domain.AssignmentsLists;

public class NameTests
{
    [Theory]
    [InlineData("N")]
    [InlineData("Name of the assignments list")]
    public async Task NameCreateFrom_WhenCalledWithProperValue_ShouldReturnExpectedObjectResultWithValue(string value)
    {
        // Arrange
        var createName = Name.CreateFrom;
        
        // Act
        var createNameResult = createName(value);
        
        // Assert
        createNameResult
            .Should()
            .Match<Result<Name>>(r => r.IsSuccess);

        createNameResult.Value!
            .Should()
            .Match<Name>(r => r.Value == value);

        await Task.CompletedTask;
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task NameCreateFrom_WhenCalledWithNullOrWhiteSpace_ShouldReturnResultWithErrorAssignmentsListNameCannotBeEmpty(string value)
    {
        // Arrange
        var createName = Name.CreateFrom;
        
        // Act
        var createNameResult = createName(value);
        
        // Assert
        createNameResult
            .Should()
            .Match<Result<Name>>(
                e => e.IsFailure
                  && e.Error == Errors.AssignmentsList.NameCannotBeEmpty
            );

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var left = Name.CreateFrom(Constants.AssignmentsLists.NameFromIndex(0).Value).Value!;
        var right = Name.CreateFrom(Constants.AssignmentsLists.NameFromIndex(0).Value).Value!;
        
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
        var left = Name.CreateFrom(Constants.AssignmentsLists.NameFromIndex(0).Value).Value!;
        var right = Name.CreateFrom(Constants.AssignmentsLists.NameFromIndex(1).Value).Value!;
        
        // Act
        var result = left.Equals(right);
        
        // Assert
        result
            .Should()
            .BeFalse();
        
        await Task.CompletedTask;
    }
}