using DoIt.Api.Domain.Shared;

namespace DoIt.Api.Domain.Tasks;

public class Title
    : ValueObject
{
    private const int titleMaxLength = 100;

    public string Value { get; private set; }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", nameof(value));

        if (value.Length > titleMaxLength)
            throw new ArgumentException("Title cannot exceed 100 characters.", nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponent()
    {
        yield return Value;
    }
}
