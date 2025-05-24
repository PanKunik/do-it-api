using DoIt.Api.Persistence.Repositories.Tasks;

namespace DoIt.Api.Persistence.Repositories.TaskLists;

public sealed class TaskListRecord(
    Guid Id,
    string Name,
    DateTime CreatedAt
)
{
    public Guid Id { get; } = Id;
    public string Name { get; } = Name;
    public DateTime CreatedAt { get; } = CreatedAt;
    public List<TaskRecord>? Tasks { get; set; }

    // public void Deconstruct(
    //     out Guid Id,
    //     out string Name,
    //     out DateTime CreatedAt,
    //     out List<TaskRecord>? Tasks
    // )
    // {
    //     Id = this.Id;
    //     Name = this.Name;
    //     CreatedAt = this.CreatedAt;
    //     Tasks = this.Tasks;
    // }
}