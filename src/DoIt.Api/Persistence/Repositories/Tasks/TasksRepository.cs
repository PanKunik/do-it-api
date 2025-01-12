using Dapper;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence.Repositories.Tasks;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Persistence.Repositories;

public class TasksRepository
    : ITasksRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TasksRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory
            ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
    }

    public async System.Threading.Tasks.Task<List<Task>> GetAll()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                task_id AS Id
                , title
                , created_at AS CreatedAt
                , is_done AS IsDone
                , is_important AS IsImportant
            FROM
                public.tasks";

        var result = await connection.QueryAsync<TaskRecord>(query);

        return result
            .Select(
                r => new Task(
                    TaskId.CreateFrom(r.Id),
                    new Title(r.Title),
                    r.CreatedAt,
                    r.IsDone,
                    r.IsImportant
                    )
            ).ToList();
    }

    public async System.Threading.Tasks.Task<Task?> GetById(TaskId taskId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                task_id AS Id
                , title
                , created_at AS CreatedAt
                , is_done AS IsDone
                , is_important AS IsImportant
            FROM
                public.tasks
            WHERE
                task_id = @Id";

        var result = await connection.QueryFirstOrDefaultAsync<TaskRecord>(query, new { Id = taskId.Value });

        if (result is null)
            return null;

        return new Task(
            TaskId.CreateFrom(result.Id),
            new Title(result.Title),
            result.CreatedAt,
            result.IsDone,
            result.IsImportant
        );
    }

    public async System.Threading.Tasks.Task<Task> Create(Task task)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            INSERT INTO public.tasks
            (
                task_id
                , title
                , created_at
                , is_done
                , is_important
            )
            VALUES
            (
                @Id
                , @Title
                , @CreatedAt
                , @IsDone
                , @isImportant
            )";

        var taskRecord = task.FromDomain()
            ?? throw new ArgumentNullException(nameof(task));

        var result = await connection.ExecuteAsync(query, taskRecord);

        if (result <= 0)
        {
            throw new InvalidOperationException("Cannot insert data to database");
        }

        return task;
    }

    public async System.Threading.Tasks.Task Delete(TaskId taskId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            DELETE FROM public.tasks
            WHERE task_id = @Id";

        var result = await connection.ExecuteAsync(query, new { Id = taskId.Value });
    }
}
