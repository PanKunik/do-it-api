using DoIt.Api.Controllers.TaskLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.TaskLists;

public interface ITaskListsService
{
    Task<Result<TaskListDto>> Create(CreateTaskListRequest request);
    Task<Result<TaskListDto>> GetById(Guid id);
}