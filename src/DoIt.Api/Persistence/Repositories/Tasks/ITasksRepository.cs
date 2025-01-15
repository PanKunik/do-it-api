using DoIt.Api.Domain.Tasks;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Persistence.Repositories;

public interface ITasksRepository
{
    System.Threading.Tasks.Task<List<Task>> GetAll();
    System.Threading.Tasks.Task<Task?> GetById(TaskId taskId);
    System.Threading.Tasks.Task<Task> Create(Task task);
    System.Threading.Tasks.Task<bool> Delete(TaskId taskId);
    System.Threading.Tasks.Task<bool> Update(Task task);
}
