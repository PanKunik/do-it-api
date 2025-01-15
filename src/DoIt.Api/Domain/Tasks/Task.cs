using DoIt.Api.Domain.Shared;

namespace DoIt.Api.Domain.Tasks;

public class Task
    : Entity<TaskId>
{
    public Title Title { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDone { get; private set; }
    public bool IsImportant { get; private set; }

    public Task(
        TaskId taskId,
        Title title,
        DateTime createdAt,
        bool isDone,
        bool isImportant
    )
        : base(taskId)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        CreatedAt = createdAt;
        IsDone = isDone;
        IsImportant = isImportant;
    }

    public void UpdateTitle(Title title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }

    public void ChangeState()
        => IsDone = !IsDone;

    public void ChangeImportance()
        => IsImportant = !IsImportant;
}
