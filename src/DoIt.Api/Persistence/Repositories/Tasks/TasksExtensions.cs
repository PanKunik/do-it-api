using DoIt.Api.Domain.Tasks;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Persistence.Repositories.Tasks;

public static class TasksExtensions
{
    public static TaskRecord? FromDomain(this Task task)
    {
        if (task is null)
            return null;

        return new TaskRecord(
            task.Id.Value,
            task.Title.Value,
            task.CreatedAt,
            task.IsDone,
            task.IsImportant
        );
    }

    public static Task ToDomain(this TaskRecord taskRecord)
    {
        return new Task(
            TaskId.CreateFrom(taskRecord.Id),
            new Title(taskRecord.Title),
            taskRecord.CreatedAt,
            taskRecord.IsDone,
            taskRecord.IsImportant
        );
    }
}
