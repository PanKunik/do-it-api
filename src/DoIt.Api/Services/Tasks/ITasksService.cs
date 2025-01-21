using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.Tasks;

public interface ITasksService
{
    Task<List<TaskDTO>> GetAll();
    Task<Result<TaskDTO>> GetById(Guid id);
    Task<Result<TaskDTO>> Create(CreateTaskRequest request);
    Task<Result<bool>> Delete(Guid id);
    Task<Result<TaskDTO>> Update(Guid id, UpdateTaskRequest request);
}