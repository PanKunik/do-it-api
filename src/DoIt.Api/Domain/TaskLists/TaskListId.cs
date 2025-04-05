using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.TaskLists;

public class TaskListId
    : ValueObject
{
    public Guid Value { get; }

    private TaskListId(Guid value)
    {
        Value = value;
    }
    
    public static Result<TaskListId> CreateFrom(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.TaskList.IdCannotBeEmpty;
        
        return new TaskListId(value);
    }
    
    // TODO: Write unit tests
    public static TaskListId CreateUnique()
        => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponent()
    {
        yield return Value;
    }
}