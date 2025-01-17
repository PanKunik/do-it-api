using DoIt.Api.Controllers.Tasks;

namespace DoIt.Api.Services.Tasks;

public interface ITasksService
{
    Task<List<GetTaskResponse>> GetAll();
    Task<GetTaskResponse?> GetById(Guid id);
    Task<CreateTaskResponse> Create(CreateTaskRequest request);
    Task<bool> Delete(Guid id);
    Task<UpdateTaskResponse?> Update(Guid id, UpdateTaskRequest request);
}