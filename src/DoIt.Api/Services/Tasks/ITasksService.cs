using DoIt.Api.Controllers.Tasks;

namespace DoIt.Api.Services.Tasks;

public interface ITasksService
{
    Task<List<GetTaskResponse>> GetAll();
    Task<CreateTaskResponse> Create(CreateTaskRequest request);
}