using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.Shared;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Services.Tasks;

public class TasksService(ITasksRepository repository)
    : ITasksService
{
    public async System.Threading.Tasks.Task<List<TaskDto>> GetAll()
    {
        var result = await repository.GetAll();
        return result
            .Select(r => r.ToDto())
            .ToList();
    }

    public async System.Threading.Tasks.Task<Result<TaskDto>> GetById(Guid id)
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        var result = await repository.GetById(taskIdResult.Value!);

        return result.Map<Result<TaskDto>>(
            onSuccess: value => value.ToDto(),
            onFailure: error => error
        );
    }

    public async System.Threading.Tasks.Task<Result<TaskDto>> Create(CreateTaskRequest request)
    {
        var taskTitleResult = Title.CreateFrom(request.Title);

        if (taskTitleResult.IsFailure)
            return taskTitleResult.Error!;

        Result<TaskListId>? taskListIdResult = null;

        if (request.TaskListId is not null)
        {
            taskListIdResult = TaskListId.CreateFrom(request.TaskListId.Value);
            
            if (taskListIdResult.IsFailure)
                return taskListIdResult.Error!;
        }
        
        // TODO: Get TaskList by id and check if exists

        var taskResult = Task.Create(
            TaskId.CreateUnique(),
            taskTitleResult.Value!,
            DateTime.UtcNow,
            false,
            false,
            taskListIdResult?.Value!
        );

        if (taskResult.IsFailure)
            return taskResult.Error!;

        var createTaskResult = await repository.Create(taskResult.Value!);

        if (createTaskResult.IsFailure)
            return createTaskResult.Error!;

        return createTaskResult.Value!.ToDto();
    }

    public async System.Threading.Tasks.Task<Result> Delete(Guid id)
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        return await repository.Delete(taskIdResult.Value!);
    }

    public async System.Threading.Tasks.Task<Result> Update(
        Guid id,
        UpdateTaskRequest request
    )
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        var titleResult = Title.CreateFrom(request.Title);

        if (titleResult.IsFailure)
            return titleResult.Error!;

        var taskToUpdateResult = await repository.GetById(taskIdResult.Value!);

        if (taskToUpdateResult.IsFailure)
            return taskToUpdateResult.Error!;

        var taskToUpdate = taskToUpdateResult.Value!;
        taskToUpdate.UpdateTitle(titleResult.Value!);

        return await repository.Update(taskToUpdate);
    }

    public async Task<Result> ChangeState(Guid id)
    {
        var taskIdResult = TaskId.CreateFrom(id);
        
        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        var taskToDoResult = await repository.GetById(taskIdResult.Value!);

        if (taskToDoResult.IsFailure)
            return taskToDoResult.Error!;

        var taskToDo = taskToDoResult.Value!;
        taskToDo.ChangeState();

        await repository.Update(taskToDo);
        
        return Result.Success();
    }
}