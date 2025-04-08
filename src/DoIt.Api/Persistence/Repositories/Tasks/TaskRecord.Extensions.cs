using DoIt.Api.Domain.TaskLists;
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
            task.IsImportant,
            task.TaskListId?.Value
        );
    }

    public static Result<Task> ToDomain(this TaskRecord taskRecord)
    {
        var taskIdResult = TaskId.CreateFrom(taskRecord.TaskId);

        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        Result<TaskListId>? taskListIdResult = null;
        
        if (taskRecord.TaskListId.HasValue)
        {
            taskListIdResult = TaskListId.CreateFrom(taskRecord.TaskListId.Value);

            if (taskListIdResult.IsFailure)
                return taskListIdResult.Error!;
        }

        var titleResult = Title.CreateFrom(taskRecord.TaskTitle);

        if (titleResult.IsFailure)
            return titleResult.Error!;

        return Task.Create(
            taskIdResult.Value!,
            titleResult.Value!,
            taskRecord.TaskCreatedAt,
            taskRecord.TaskIsDone,
            taskRecord.TaskIsImportant,
            taskListIdResult?.Value!
        );
    }
}
