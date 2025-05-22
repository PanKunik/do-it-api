using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.Shared;

namespace DoIt.Api.Persistence.Repositories.TaskLists;

public static class Extensions
{
    public static Result<TaskListRecord> FromDomain(this TaskList taskList)
    {
        var taskListRecord = new TaskListRecord(
            taskList.Id.Value,
            taskList.Name.Value,
            taskList.CreatedAt
        );

        taskListRecord.Tasks = taskList.Tasks
            .Select(t => t.FromDomain().Value!)
            .ToList();
        return taskListRecord;
    }

    public static Result<TaskList> ToDomain(this TaskListRecord taskListRecord)
    {
        var taskListIdResult = TaskListId.CreateFrom(taskListRecord.TaskListId);
        
        if (taskListIdResult.IsFailure)
            return taskListIdResult.Error!;
        
        var taskListNameResult = Name.CreateFrom(taskListRecord.TaskListName);
        
        if (taskListNameResult.IsFailure)
            return taskListNameResult.Error!;

        return TaskList.Create(
            taskListIdResult.Value!,
            taskListNameResult.Value!,
            taskListRecord.TaskListCreatedAt,
            taskListRecord.Tasks
                .Select(t => t.ToDomain().Value!)
                .ToList()  // TODO: What if result is false?
        );
    }
}