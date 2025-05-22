using DoIt.Api.Shared;
using DoIt.Api.Shared.Errors;
using NSubstitute;

namespace DoIt.Api.Unit.Tests.Shared;
public class ResultTests
{
    [Fact]
    public async Task ResultSuccess_WhenInvoked_ShouldReturnResultObject()
    {
        // Arrange
        var createSuccessResult = Result.Success;

        // Act
        var cut = createSuccessResult();

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut
            .Should()
            .BeOfType<Result>();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultSuccess_WhenInvoked_ShouldSetIsSuccessfullyEqualToTrue()
    {
        // Arrange
        var createSuccessResult = Result.Success;

        // Act
        var cut = createSuccessResult();

        // Assert
        cut.IsSuccess
            .Should()
            .BeTrue();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultSuccess_WhenInvoked_ShouldSetIsFailureEqualToFalse()
    {
        // Arrange
        var createSuccessResult = Result.Success;

        // Act
        var cut = createSuccessResult();

        // Assert
        cut.IsFailure
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultSuccess_WhenInvoked_ShouldSetErrorToNull()
    {
        // Arrange
        var createSuccessResult = Result.Success;

        // Act
        var cut = createSuccessResult();

        // Assert
        cut.Error
            .Should()
            .BeNull();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultFailure_WhenInvokedWithError_ShouldReturnResultObject()
    {
        // Arrange
        var createFailureResult = Result.Failure;

        // Act
        var cut = createFailureResult(
            Error.Failure(
                "GeneralFailure",
                "Message"
            )
        );

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut
            .Should()
            .BeOfType<Result>();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultFailure_WhenInvokedWithError_ShouldSetIsSuccessToFalse()
    {
        // Arrange
        var createFailureResult = Result.Failure;

        // Act
        var cut = createFailureResult(
            Error.Failure(
                "GeneralFailure",
                "Message"
            )
        );

        // Assert
        cut.IsSuccess
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultFailure_WhenInvokedWithError_ShouldSetIsFailureToTrue()
    {
        // Arrange
        var createFailureResult = Result.Failure;

        // Act
        var cut = createFailureResult(
            Error.Failure(
                "GeneralFailure", 
                "Message"
            )
        );

        // Assert
        cut.IsFailure
            .Should()
            .BeTrue();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultFailure_WhenInvokedWithError_ShouldSetErrorToExpectedValue()
    {
        // Arrange
        var createFailureResult = Result.Failure;

        // Act
        var cut = createFailureResult(
            Error.Failure(
                "GeneralFailure", 
                "Message"
            )
        );

        // Assert
        cut.Error
            .Should()
            .NotBeNull();

        cut.Error
            .Should()
            .Match<Error>(
                e => e.Code == "GeneralFailure"
                  && e.Message == "Message"
            );

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultMap_WhenInvokedForSuccessfullyResult_ShouldCallOnSuccessOnce()
    {
        // Arrange
        var createSuccessResult = Result.Success;
        var cut = createSuccessResult();

        var success = Substitute.For<Func<int>>();
        var failure = Substitute.For<Func<Error, int>>();

        // Act
        cut.Map(
            onSuccess: success,
            onFailure: failure
        );

        // Assert
        success
            .Received(1);

        failure
            .Received(0);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultMap_WhenInvokedForFailedResult_ShouldCallOnFailureOnce()
    {
        // Arrange
        var createSuccessResult = Result.Failure;
        var cut = createSuccessResult(
            Error.Failure(
                "Code", 
                "Message"
            )
        );

        var success = Substitute.For<Func<int>>();
        var failure = Substitute.For<Func<Error, int>>();

        // Act
        cut.Map(
            onSuccess: success,
            onFailure: failure
        );

        // Assert
        failure
            .Received(1);

        success
            .Received(0);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultImplicitOperatorError_WhenInvokedForFailedResult_ShouldCastToResultWithError()
    {
        // Arrange
        Error error = Error.Failure(
            "Code", 
            "Message"
        );

        // Act
        Result result = error;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<Result>();

        result.IsSuccess
            .Should()
            .BeFalse();

        result.IsFailure
            .Should()
            .BeTrue();

        result.Error
            .Should()
            .NotBeNull();

        result.Error 
            .Should()
            .Match<Error>(
                e => e.Code == "Code"
                  && e.Message == "Message"
            );

        await Task.CompletedTask;
    }
}