using DoIt.Api.Domain.Tasks;
using DoIt.Api.Shared;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Persistence.Repositories.Tasks;

public interface ITasksRepository
{
    System.Threading.Tasks.Task<List<Task>> GetAll();
    System.Threading.Tasks.Task<Result<Task>> GetById(TaskId taskId);
    System.Threading.Tasks.Task<Result<Task>> Create(Task task);
    System.Threading.Tasks.Task<Result> Delete(TaskId taskId);
    System.Threading.Tasks.Task<Result> Update(Task task);
}