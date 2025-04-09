using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Domain.TaskLists;

public class TaskList
    : Entity<TaskListId>
{
    public Name Name { get; }
    public DateTime CreatedAt { get; }
    public List<Task> Tasks { get; }
    
    private TaskList(
        TaskListId taskListId,
        Name name,
        DateTime createdAt,
        List<Task> tasks
    )
        : base(taskListId)
    {
        Name = name;
        CreatedAt = createdAt;
        Tasks = tasks;
    }

    public static Result<TaskList> Create(
        TaskListId taskListId,
        Name name,
        DateTime createdAt,
        List<Task>? tasks = null
    )
    {
        if (taskListId is null)
            return Errors.TaskList.IdCannotBeNull;
        
        if (name is null)
            return Errors.TaskList.NameCannotBeNull;
        
        return new TaskList(
            taskListId,
            name,
            createdAt,
            tasks ?? []
        );
    }
}