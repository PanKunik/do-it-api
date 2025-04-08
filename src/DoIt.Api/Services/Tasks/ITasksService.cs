using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.Tasks;

public interface ITasksService
{
    Task<List<TaskDto>> GetAll();
    Task<Result<TaskDto>> GetById(Guid id);
    Task<Result<TaskDto>> Create(CreateTaskRequest request);
    Task<Result> Delete(Guid id);
    Task<Result> Update(Guid id, UpdateTaskRequest request);
    Task<Result> ChangeState(Guid id);
}