using DoIt.Api.Domain.Shared;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Tasks;

public class Task
    : Entity<TaskId>
{
    public Title Title { get; private set; }
    public DateTime CreatedAt { get; }
    public bool IsDone { get; private set; }
    public bool IsImportant { get; private set; }

    private Task(
        TaskId taskId,
        Title title,
        DateTime createdAt,
        bool isDone,
        bool isImportant
    )
        : base(taskId)
    {
        Title = title;
        CreatedAt = createdAt;
        IsDone = isDone;
        IsImportant = isImportant;
    }

    public static Result<Task> Create(
        TaskId taskId,
        Title title,
        DateTime createdAt,
        bool isDone,
        bool isImportant
    )
    {
        if (taskId is null)
            return Errors.Task.NullTaskId;

        if (title is null)
            return Errors.Task.NullTitle;

        return new Task(
            taskId,
            title,
            createdAt,
            isDone,
            isImportant
        );
    }

    // TODO: Result pattern (non-generic)
    public void UpdateTitle(Title title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }


    // TODO: Result pattern (non-generic)
    public void ChangeState()
        => IsDone = !IsDone;


    // TODO: Result pattern (non-generic)
    public void ChangeImportance()
        => IsImportant = !IsImportant;
}
