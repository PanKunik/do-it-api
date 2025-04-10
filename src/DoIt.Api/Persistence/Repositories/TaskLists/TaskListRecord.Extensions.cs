using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Persistence.Repositories.TaskLists;

public static class Extensions
{
    public static Result<TaskListRecord> FromDomain(this TaskList taskList)
    {
        return new TaskListRecord(
            taskList.Id.Value,
            taskList.Name.Value,
            taskList.CreatedAt
        );
    }

    public static Result<TaskList> ToDomain(this TaskListRecord taskListRecord)
    {
        var taskListIdResult = TaskListId.CreateFrom(taskListRecord.Id);
        
        if (taskListIdResult.IsFailure)
            return taskListIdResult.Error!;
        
        var taskListNameResult = Name.CreateFrom(taskListRecord.Name);
        
        if (taskListNameResult.IsFailure)
            return taskListNameResult.Error!;

        return TaskList.Create(
            taskListIdResult.Value!,
            taskListNameResult.Value!,
            taskListRecord.CreatedAt
        );
    }
}