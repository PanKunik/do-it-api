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
        var taskId = TaskId.CreateFrom(id);
        var result = await _repository.GetById(taskId);

        if (result is null)
            return Errors.Task.NotFound;

        return result.ToDTO();
    }

    public async System.Threading.Tasks.Task<Result<TaskDTO>> Create(CreateTaskRequest request)
    {
        var newTask = new Task(
            TaskId.CreateFrom(Guid.NewGuid()),
            new Title(request.Title),
            DateTime.UtcNow,
            false,
            false
        );

        var result = await _repository.Create(newTask);

        return result.ToDTO();
    }

    public async System.Threading.Tasks.Task<Result<bool>> Delete(Guid id)
    {
        var taskId = TaskId.CreateFrom(id);
        var result = await _repository.Delete(taskId);
        return result
            ? result
            : Errors.Task.NotFound;
    }

    public async System.Threading.Tasks.Task<Result<TaskDTO>> Update(Guid id, UpdateTaskRequest request)
    {
        var taskId = TaskId.CreateFrom(id);
        var title = new Title(request.Title);
        var taskToUpdate = await _repository.GetById(taskId);

        if (taskToUpdate is null)
            return Errors.Task.NotFound;

        taskToUpdate.UpdateTitle(title);

        var result = await _repository.Update(taskToUpdate);

        return taskToUpdate.ToDTO();
    }
}
