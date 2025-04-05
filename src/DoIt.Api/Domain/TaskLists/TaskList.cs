using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.TaskLists;

public class TaskList
    : Entity<TaskListId>
{
    public Name Name { get; }
    public DateTime CreatedAt { get; }
    
    private TaskList(
        TaskListId taskListId,
        Name name,
        DateTime createdAt
    )
        : base(taskListId)
    {
        Name = name;
        CreatedAt = createdAt;
    }

    public static Result<TaskList> Create(
        TaskListId taskListId,
        Name name,
        DateTime createdAt
    )
    {
        if (taskListId is null)
            return Errors.TaskList.IdCannotBeNull;
        
        if (name is null)
            return Errors.TaskList.NameCannotBeNull;
        
        return new TaskList(
            taskListId,
            name,
            createdAt
        );
    }
}