namespace DoIt.Api.Persistence.Repositories
{
    public interface ITasksRepository
    {
        Task<List<TaskDTO>> GetAll();
    }
}
