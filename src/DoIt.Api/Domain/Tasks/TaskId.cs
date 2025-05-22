using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Tasks;

public class TaskId
    : ValueObject
{
    public Guid Value { get; }

    private TaskId(Guid value)
    {
        Value = value;
    }

    public static Result<TaskId> CreateFrom(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.Task.IdCannotBeEmpty;

        return new TaskId(value);
    }

    // TODO: Write unit tests
    public static TaskId CreateUnique()
        => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponent()
    {
        yield return Value;
    }
}