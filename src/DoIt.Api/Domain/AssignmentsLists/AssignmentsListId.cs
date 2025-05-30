using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.AssignmentsLists;

public class AssignmentsListId
    : ValueObject
{
    public Guid Value { get; }

    private AssignmentsListId(Guid value)
    {
        Value = value;
    }
    
    public static Result<AssignmentsListId> CreateFrom(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.AssignmentsList.IdCannotBeEmpty;
        
        return new AssignmentsListId(value);
    }
    
    // TODO: Write unit tests
    public static AssignmentsListId CreateUnique()
        => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponent()
    {
        yield return Value;
    }
}