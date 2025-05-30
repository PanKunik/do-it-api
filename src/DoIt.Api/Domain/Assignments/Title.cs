using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Assignments;

public class Title
    : ValueObject
{
    private const int TitleMaxLength = 100;

    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title> CreateFrom(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Assignment.TitleCannotBeEmpty;

        if (value.Length > TitleMaxLength)
            return Errors.Assignment.TitleTooLong;

        return new Title(value);
    }

    protected override IEnumerable<object> GetEqualityComponent()
    {
        yield return Value;
    }
}