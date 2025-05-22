using DoIt.Api.Domain.Shared;
using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Tasks;

public class Task
    : Entity<TaskId>
{
    public Title Title { get; private set; }
    public DateTime CreatedAt { get; }
    public bool IsDone { get; private set; }
    public bool IsImportant { get; private set; }
    public TaskListId? TaskListId { get; }

    private Task(
        TaskId taskId,
        Title title,
        DateTime createdAt,
        bool isDone,
        bool isImportant,
        TaskListId? taskListId
    )
        : base(taskId)
    {
        Title = title;
        CreatedAt = createdAt;
        IsDone = isDone;
        IsImportant = isImportant;
        TaskListId = taskListId;
    }

    public static Result<Task> Create(
        TaskId taskId,
        Title title,
        DateTime createdAt,
        bool isDone,
        bool isImportant,
        TaskListId? taskListId
    )
    {
        // TODO: Force not null
        if (taskId is null)
            return Errors.Task.IdCannotBeNull;

        // TODO: Force not null
        if (title is null)
            return Errors.Task.TitleCannotBeNull;

        return new Task(
            taskId,
            title,
            createdAt,
            isDone,
            isImportant,
            taskListId
        );
    }

    public Result UpdateTitle(Title title)
    {
        // TODO: Force not null
        if (title is null)
            return Result.Failure(Errors.Task.TitleCannotBeNull);

        Title = title;

        return Result.Success();
    }

    public void ChangeState()
        => IsDone = !IsDone;

    public void ChangeImportance()
        => IsImportant = !IsImportant;
}