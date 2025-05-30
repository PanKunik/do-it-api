using DoIt.Api.Domain;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;
using DoIt.Api.TestUtils;

namespace DoIt.Api.Unit.Tests.Domain.Assignments;

public class TitleTests
{
    [Theory]
    [InlineData("T")]
    [InlineData("Task title")]
    [InlineData("M%@HUf3Hp#7AG%JyweXCvwaOWaRw2aHY+#HGp#tQCu$+YtxbG9WysXKr*xOw0vJey@XTst3#1BTOubDN5F2OYOtoHfEc15FOq3Qs")]
    public async Task TitleCreateFrom_WhenCalledWithProperValue_ShouldReturnExpectedObjectResultWithValue(string value)
    {
        // Arrange
        var createTitle = Title.CreateFrom;

        // Act
        var createTitleResult = createTitle(value);

        // Assert
        createTitleResult
            .Should()
            .Match<Result<Title>>(r => r.IsSuccess);

        createTitleResult.Value!
            .Should()
            .Match<Title>(r => r.Value == value);

        await Task.CompletedTask;
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Title_WhenPassedNullOrWhiteSpace_ShouldReturnResultWithErrorAssignmentTitleCannotBeEmpty(string value)
    {
        // Arrange
        var createTitle = Title.CreateFrom;

        // Act
        var createTitleResult = createTitle(value);

        // Assert
        createTitleResult
            .Should()
            .Match<Result<Title>>(
                e => e.IsFailure
                  && e.Error == Errors.Assignment.TitleCannotBeEmpty
            );

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Title_WhenPassedValueWithOver100Characters_ShouldReturnResultWithErrorAssignmentTitleTooLong()
    {
        // Arrange
        var createTitle = Title.CreateFrom;

        // Act
        var createTitleResult = createTitle("dwWY3pM5eakP3qbsku37HrW3bMEaA@%T9Q+aKZeRW%FzWwwucpjnFRXU2q9$pH!$j#M+azz72WQ&4vrFbw*8%eca5r$kps48d%REs");

        // Assert
        createTitleResult
            .Should()
            .Match<Result<Title>>(
                e => e.IsFailure
                  && e.Error == Errors.Assignment.TitleTooLong
            );

        await Task.CompletedTask;
    }

    [Fact]
    public async Task Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var left = Title.CreateFrom(Constants.Tasks.Title.Value).Value!;
        var right = Title.CreateFrom(Constants.Tasks.Title.Value).Value!;

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
        var left = Title.CreateFrom(Constants.Tasks.TitleFromIndex(0).Value).Value!;
        var right = Title.CreateFrom(Constants.Tasks.TitleFromIndex(1).Value).Value!;

        // Act
        var result = left.Equals(right);

        // Assert
        result
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }
}