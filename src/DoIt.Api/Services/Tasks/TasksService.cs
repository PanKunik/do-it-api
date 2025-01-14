using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Repositories;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Services.Tasks;

public class TasksService(ITasksRepository repository)
    : ITasksService
{
    private readonly ITasksRepository _repository = repository;

    public async System.Threading.Tasks.Task<List<GetTaskResponse>> GetAll()
    {
        var result = await _repository.GetAll();
        return result
            .Select(
                r => new GetTaskResponse(
                    r.Id.Value,
                    r.Title.Value,
                    r.CreatedAt,
                    r.IsDone,
                    r.IsImportant
                )
            ).ToList();
    }

    public async System.Threading.Tasks.Task<GetTaskResponse?> GetById(Guid id)
    {
        var taskId = TaskId.CreateFrom(id);
        var result = await _repository.GetById(taskId);

        if (result is null)
            return null;

        return new GetTaskResponse(
                result.Id.Value,
                result.Title.Value,
                result.CreatedAt,
                result.IsDone,
                result.IsImportant
            );
    }

    public async System.Threading.Tasks.Task<CreateTaskResponse> Create(CreateTaskRequest request)
    {
        var newTask = new Task(
            TaskId.CreateFrom(Guid.NewGuid()),
            new Title(request.Title),
            DateTime.UtcNow,
            false,
            false
        );

        var result = await _repository.Create(newTask);
        return new CreateTaskResponse(
            result.Id.Value,
            result.Title.Value,
            result.CreatedAt,
            result.IsDone,
            result.IsImportant
        );
    }

    public async System.Threading.Tasks.Task<bool> Delete(Guid id)
    {
        var taskId = TaskId.CreateFrom(id);
        return await _repository.Delete(taskId);
    }
}
