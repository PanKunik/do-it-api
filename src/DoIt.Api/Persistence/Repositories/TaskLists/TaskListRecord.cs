using DoIt.Api.Persistence.Repositories.Tasks;

namespace DoIt.Api.Persistence.Repositories.TaskLists;

// TODO: Find better naming for fields
public sealed class TaskListRecord(
    Guid TaskListId,
    string TaskListName,
    DateTime TaskListCreatedAt
)
{
    public Guid TaskListId { get; init; } = TaskListId;
    public string TaskListName { get; init; } = TaskListName;
    public DateTime TaskListCreatedAt { get; init; } = TaskListCreatedAt;
    public List<TaskRecord>? Tasks { get; set; }

    public void Deconstruct(
        out Guid TaskListId,
        out string TaskListName,
        out DateTime TaskListCreatedAt,
        out List<TaskRecord>? Tasks
    )
    {
        TaskListId = this.TaskListId;
        TaskListName = this.TaskListName;
        TaskListCreatedAt = this.TaskListCreatedAt;
        Tasks = this.Tasks;
    }
}