using DoIt.Api.Shared.Enums;

namespace DoIt.Api.Shared;

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
