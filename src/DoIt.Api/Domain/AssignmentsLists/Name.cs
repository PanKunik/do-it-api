using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.AssignmentsLists;

public class Name
    : ValueObject
{
    private const int NameMaxLength = 100;
    
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }
    
    public static Result<Name> CreateFrom(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.AssignmentsList.NameCannotBeEmpty;

        if (value.Length > NameMaxLength)
            return Errors.AssignmentsList.NameTooLong;
        
        return new Name(value);
    }

    protected override IEnumerable<object> GetEqualityComponent()
    {
        yield return Value;
    }
}