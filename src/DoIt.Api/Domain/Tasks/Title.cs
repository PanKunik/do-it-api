using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Tasks;

public class Title
    : ValueObject
{
    private const int titleMaxLength = 100;

    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title> CreateFrom(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Task.TitleCannotBeEmpty;

        if (value.Length > titleMaxLength)
            return Errors.Task.TitleTooLong;

        return new Title(value);
    }

    protected override IEnumerable<object> GetEqualityComponent()
    {
        yield return Value;
    }
}
