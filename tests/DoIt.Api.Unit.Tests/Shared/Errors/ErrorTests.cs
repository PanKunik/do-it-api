using DoIt.Api.Shared.Errors;
using DoIt.Api.Shared.Errors.Enums;

namespace DoIt.Api.Unit.Tests.Shared.Errors;

public class ErrorTests
{
    [Theory]
    [InlineData("Code", "Message")]
    [InlineData("Error.NotFound", "Entity wasn't found")]
    public async Task Error_WhenCalled_ShouldSetCodeAndMessage(
        string code,
        string message
    )
    {
        // Arrange
        var createError = Error.Failure;

        // Act
        var cut = createError(
            code,
            message
        );

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut.Code
            .Should()
            .Be(code);

        cut.Message
            .Should()
            .Be(message);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ErrorFailure_WhenInvokedWithProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createFailureError = Error.Failure;

        // Act
        var cut = createFailureError("Code", "Message");

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut
            .Should()
            .BeOfType<Error>();

        cut
            .Should()
            .Match<Error>(
                e => e.Code == "Code"
                  && e.Message == "Message"
                  && e.Type == ErrorType.Failure
            );

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ErrorValidation_WhenInvokedWithProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createValidationError = Error.Validation;

        // Act
        var cut = createValidationError("Code", "Message");

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut
            .Should()
            .BeOfType<Error>();

        cut
            .Should()
            .Match<Error>(
                e => e.Code == "Code"
                  && e.Message == "Message"
                  && e.Type == ErrorType.Validation
            );

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ErrorNotFound_WhenInvokedWithProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createNotFoundError = Error.NotFound;

        // Act
        var cut = createNotFoundError("Code", "Message");

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut
            .Should()
            .BeOfType<Error>();

        cut
            .Should()
            .Match<Error>(
                e => e.Code == "Code"
                  && e.Message == "Message"
                  && e.Type == ErrorType.NotFound
            );

        await Task.CompletedTask;
    }
}
