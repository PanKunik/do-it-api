using DoIt.Api.Controllers.TaskLists;
using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Persistence.Repositories.TaskLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.TaskLists;

public class TaskListsService(ITaskListsRepository repository)
    : ITaskListsService
{
    public async Task<Result<TaskListDTO>> Create(CreateTaskListRequest request)
    {
        var taskListNameResult = Name.CreateFrom(request.Name);
        
        if (taskListNameResult.IsFailure)
            return taskListNameResult.Error!;

        var taskListResult = TaskList.Create(
            TaskListId.CreateUnique(),
            taskListNameResult.Value!,
            DateTime.Now
        );
        
        if (taskListResult.IsFailure)
            return taskListResult.Error!;

        var createTaskListResult = await repository.Create(taskListResult.Value!);

        if (createTaskListResult.IsFailure)
            return createTaskListResult.Error!;

        return createTaskListResult.Value!.ToDto();
    }
}