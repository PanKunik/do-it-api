using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Repositories;
using DoIt.Api.Shared;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Services.Tasks;

public class TasksService(ITasksRepository repository)
    : ITasksService
{
    private readonly ITasksRepository _repository = repository;

    public async System.Threading.Tasks.Task<List<TaskDTO>> GetAll()
    {
        var result = await _repository.GetAll();
        return result
            .Select(r => r.ToDTO())
            .ToList();
    }

    public async System.Threading.Tasks.Task<Result<TaskDTO>> GetById(Guid id)
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (!taskIdResult.IsSuccess)
            return taskIdResult.Error!;

        var result = await _repository.GetById(taskIdResult.Value!);

        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!.ToDTO();
    }

    public async System.Threading.Tasks.Task<Result<TaskDTO>> Create(CreateTaskRequest request)
    {
        var taskIdResult = TaskId.CreateFrom(Guid.NewGuid());

        if (!taskIdResult.IsSuccess)
            return taskIdResult.Error!;

        var taskTitleResult = Title.CreateFrom(request.Title);

        if (!taskTitleResult.IsSuccess)
            return taskTitleResult.Error!;

        var taskResult = Task.Create(
            taskIdResult.Value!,
            taskTitleResult.Value!,
            DateTime.UtcNow,
            false,
            false
        );

        if (!taskResult.IsSuccess)
            return taskResult.Error!;

        var result = await _repository.Create(taskResult.Value!);

        return result.Value!.ToDTO();
    }

    public async System.Threading.Tasks.Task<Result<bool>> Delete(Guid id)
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (!taskIdResult.IsSuccess)
            return taskIdResult.Error!;

        var result = await _repository.Delete(taskIdResult.Value!);

        return result
            ? result
            : Errors.Task.NotFound;
    }

    public async System.Threading.Tasks.Task<Result<TaskDTO>> Update(Guid id, UpdateTaskRequest request)
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (!taskIdResult.IsSuccess)
            return taskIdResult.Error!;

        var titleResult = Title.CreateFrom(request.Title);

        if (!titleResult.IsSuccess)
            return titleResult.Error!;

        var taskToUpdate = await _repository.GetById(taskIdResult.Value!);

        if (!taskToUpdate.IsSuccess)
            return taskToUpdate.Error!;

        taskToUpdate.Value!.UpdateTitle(titleResult.Value!);

        var result = await _repository.Update(taskToUpdate.Value!);

        return taskToUpdate.Value!.ToDTO();
    }
}
