using DoIt.Api.Shared.Errors.Enums;

namespace DoIt.Api.Shared.Errors;

public sealed record class Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(
        string code,
        string message,
        ErrorType type
    )
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Failure(
        string code,
        string message
    )
    {
        return new Error(code, message, ErrorType.Failure);
    }

    public static Error Validation(
        string code,
        string message
    )
    {
        return new(code, message, ErrorType.Validation);
    }

    public static Error NotFound(
        string code,
        string message
    )
    {
        return new(code, message, ErrorType.NotFound);
    }
}
