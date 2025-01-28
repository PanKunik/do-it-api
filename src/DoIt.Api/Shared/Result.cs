using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Shared;

// TODO: Write tests
public sealed class Result<T> : Result
{
    private Result(T value)
        : base()
    {
        Value = value;
    }

    private Result(Error error)
        : base(error)
    {
        Value = default;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new Result<T>(value);
    public new static Result<T> Failure(Error error) => new Result<T>(error);

    public static implicit operator Result<T>(T value) => new Result<T>(value);
    public static implicit operator Result<T>(Error error) => new Result<T>(error);

    public TResult Map<TResult>(
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure
    )
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }
}

// TODO: Write tests
public class Result
{
    protected Result()
    {
        Error = null;
    }

    protected Result(Error error)
    {
        Error = error;
    }

    public Error? Error { get; protected set; }
    public bool IsSuccess => Error == null;
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new Result();
    public static Result Failure(Error error) => new Result(error);

    public static implicit operator Result(Error error) => new Result(error);

    public TResult Map<TResult>(
        Func<TResult> onSuccess,
        Func<Error, TResult> onFailure
    )
    {
        return IsSuccess ? onSuccess() : onFailure(Error!);
    }
}