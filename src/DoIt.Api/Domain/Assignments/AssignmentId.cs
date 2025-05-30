using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Assignments;

public class AssignmentId
    : ValueObject
{
    public Guid Value { get; }

    private AssignmentId(Guid value)
    {
        Value = value;
    }

    public static Result<AssignmentId> CreateFrom(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.Assignment.IdCannotBeEmpty;

        return new AssignmentId(value);
    }

    // TODO: Write unit tests
    public static AssignmentId CreateUnique()
        => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponent()
    {
        yield return Value;
    }
}