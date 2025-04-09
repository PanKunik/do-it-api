using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Persistence.Repositories.TaskLists;

public interface ITaskListsRepository
{
    Task<Result<TaskList>> Create(TaskList taskList);
    Task<Result<TaskList>> GetById(TaskListId taskListId);
}