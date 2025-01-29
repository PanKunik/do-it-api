using DoIt.Api.Shared;
using DoIt.Api.Shared.Errors;
using NSubstitute;

namespace DoIt.Api.Unit.Tests.Shared;
public class GenericResultTests
{
    [Fact]
    public async Task ResultTSuccess_WhenInvoked_ShouldReturnResultObject()
    {
        // Arrange
        Func<int, Result<int>> createSuccessResult = Result<int>.Success;

        // Act
        var cut = createSuccessResult(1);

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut
            .Should()
            .BeOfType<Result<int>>();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTSuccess_WhenInvoked_ShouldSetIsSuccessfullEqualToTrue()
    {
        // Arrange
        Func<int, Result<int>> createSuccessResult = Result<int>.Success;

        // Act
        var cut = createSuccessResult(1);

        // Assert
        cut.IsSuccess
            .Should()
            .BeTrue();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTSuccess_WhenInvoked_ShouldSetIsFailureEqualToTrue()
    {
        // Arrange
        Func<int, Result<int>> createSuccessResult = Result<int>.Success;

        // Act
        var cut = createSuccessResult(1);

        // Assert
        cut.IsFailure
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTSuccess_WhenInvoked_ShouldSetErrorToNull()
    {
        // Arrange
        Func<int, Result<int>> createSuccessResult = Result<int>.Success;

        // Act
        var cut = createSuccessResult(1);

        // Assert
        cut.Error
            .Should()
            .BeNull();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTSuccess_WhenInvoked_ShouldSetValueToExpectedValue()
    {
        // Arrange
        Func<int, Result<int>> createSuccessResult = Result<int>.Success;

        // Act
        var cut = createSuccessResult(1);

        // Assert
        cut.Value
            .Should()
            .Be(1);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTFailure_WhenInvokedWithError_ShouldReturnResultObject()
    {
        // Arrange
        Func<Error, Result<int>> createFailedResult = Result<int>.Failure;

        // Act
        var cut = createFailedResult(Error.Failure("Code", "Message"));

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut
            .Should()
            .BeOfType<Result<int>>();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTFailure_WhenInvokedWithError_ShouldSetIsSuccessToFalse()
    {
        // Arrange
        Func<Error, Result<int>> createFailedResult = Result<int>.Failure;

        // Act
        var cut = createFailedResult(Error.Failure("Code", "Message"));

        // Assert
        cut.IsSuccess
            .Should()
            .BeFalse();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTFailure_WhenInvokedWithError_ShouldSetIsFailureToFalse()
    {
        // Arrange
        Func<Error, Result<int>> createFailedResult = Result<int>.Failure;

        // Act
        var cut = createFailedResult(Error.Failure("Code", "Message"));

        // Assert
        cut.IsFailure
            .Should()
            .BeTrue();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTFailure_WhenInvokedWithError_ShouldSetErrorToExpectedValue()
    {
        // Arrange
        Func<Error, Result<int>> createFailedResult = Result<int>.Failure;

        // Act
        var cut = createFailedResult(Error.Failure("Code", "Message"));

        // Assert
        cut.Error
            .Should()
            .NotBeNull();

        cut.Error
            .Should()
            .Match<Error>(
                e => e.Code == "Code"
                  && e.Message == "Message"
            );

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTFailure_WhenInvokedWithErrorAsValueType_ShouldSetValueToDefault()
    {
        // Arrange
        Func<Error, Result<int>> createFailedResult = Result<int>.Failure;

        // Act
        var cut = createFailedResult(Error.Failure("Code", "Message"));

        // Assert
        cut.Value
            .Should()
            .Be(0);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTFailure_WhenInvokedWithErrorAsReferenceType_ShouldSetValueToNull()
    {
        // Arrange
        Func<Error, Result<object>> createFailedResult = Result<object>.Failure;

        // Act
        var cut = createFailedResult(Error.Failure("Code", "Message"));

        // Assert
        cut.Value
            .Should()
            .BeNull();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTMap_WhenInvokedForSuccessfullResult_ShouldCallOnSuccessOnce()
    {
        // Arrange
        Func<int, Result<int>> createSuccessResult = Result<int>.Success;
        var cut = createSuccessResult(1);

        var success = Substitute.For<Func<int, int>>();
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
    public async Task ResultTMap_WhenInvokedForFailedResult_ShouldCallOnFailureOnce()
    {
        // Arrange
        Func<Error, Result<int>> createSuccessResult = Result<int>.Failure;
        var cut = createSuccessResult(Error.Failure("Code", "Message"));

        var success = Substitute.For<Func<int, int>>();
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
    public async Task ResultTImplicitOperatorValue_WhenInvokedForSuccessResultAsValueType_ShouldCastToResultWithValue()
    {
        // Arrange
        int value = 1;

        // Act
        Result<int> result = value;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<Result<int>>();

        result.IsSuccess
            .Should()
            .BeTrue();

        result.IsFailure
            .Should()
            .BeFalse();

        result.Error
            .Should()
            .BeNull();

        result.Value
            .Should()
            .Be(1);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTImplicitOperatorValue_WhenInvokedForSuccessResultAsReferenceType_ShouldCastToResultWithValue()
    {
        // Arrange
        object value = new { A = 1 };

        // Act
        Result<object> result = value;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<Result<object>>();

        result.IsSuccess
            .Should()
            .BeTrue();

        result.IsFailure
            .Should()
            .BeFalse();

        result.Error
            .Should()
            .BeNull();

        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeEquivalentTo<object>(new { A = 1 });

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTImplicitOperatorError_WhenInvokedForFailedResultAsValueType_ShouldCastToResultWithError()
    {
        // Arrange
        Error error = Error.Failure("Code", "Message");

        // Act
        Result<int> result = error;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<Result<int>>();

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

        result.Value
            .Should()
            .Be(0);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task ResultTImplicitOperatorError_WhenInvokedForFailedResultAsReferenceType_ShouldCastToResultWithError()
    {
        // Arrange
        Error error = Error.Failure("Code", "Message");

        // Act
        Result<object> result = error;

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<Result<object>>();

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

        result.Value
            .Should()
            .BeNull();

        await Task.CompletedTask;
    }
}
