using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Services.Tasks;

public static class TasksExtensions
{
    public static TaskDTO ToDTO(this Task task)
        => new(
            task.Id.Value,
            task.Title.Value,
            task.CreatedAt,
            task.IsDone,
            task.IsImportant
        );
}
