using DoIt.Api.Domain.Tasks;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Persistence.Repositories;

public interface ITasksRepository
{
    System.Threading.Tasks.Task<List<Task>> GetAll();
    System.Threading.Tasks.Task<Task> Create(Task task);
}
