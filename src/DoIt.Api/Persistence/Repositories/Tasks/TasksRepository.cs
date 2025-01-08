using Dapper;
using DoIt.Api.Persistence.Database;

namespace DoIt.Api.Persistence.Repositories;

public class TasksRepository
    : ITasksRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TasksRepository(
        IDbConnectionFactory dbConnectionFactory    
    )
    {
        _dbConnectionFactory = dbConnectionFactory
            ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
    }

    public async Task<List<TaskDTO>> GetAll()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                task_id AS taskId
                , title
                , created_at AS createdAt
                , is_done AS isDone
                , is_important AS isImportant
            FROM
                public.tasks";

        var result = await connection.QueryAsync<TaskDTO>(query);

        return result.ToList();
    }
}
