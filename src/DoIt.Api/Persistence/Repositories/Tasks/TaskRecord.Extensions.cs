using DoIt.Api.Domain.Tasks;
using DoIt.Api.Shared;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Persistence.Repositories.Tasks;

public static class Extensions
{
    public static Result<TaskRecord> FromDomain(this Task task)
    {
        return new TaskRecord(
            task.Id.Value,
            task.Title.Value,
            task.CreatedAt,
            task.IsDone,
            task.IsImportant
        );
    }

    public static Result<Task> ToDomain(this TaskRecord taskRecord)
    {
        var taskIdResult = TaskId.CreateFrom(taskRecord.Id);

        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        var titleResult = Title.CreateFrom(taskRecord.Title);

        if (titleResult.IsFailure)
            return titleResult.Error!;

        return Task.Create(
            taskIdResult.Value!,
            titleResult.Value!,
            taskRecord.CreatedAt,
            taskRecord.IsDone,
            taskRecord.IsImportant
        );
    }
}
