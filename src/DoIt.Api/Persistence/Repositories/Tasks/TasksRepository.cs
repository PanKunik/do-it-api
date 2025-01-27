using Dapper;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.Shared;
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
            .Select(r => r.ToDomain().Value!)
            .ToList();
    }

    public async System.Threading.Tasks.Task<Result<Task>> GetById(TaskId taskId)
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

        var existingTask = await connection.QueryFirstOrDefaultAsync<TaskRecord>(query, new { Id = taskId.Value });

        if (existingTask is null)
            return Errors.Task.NotFound;

        var taskResult = existingTask.ToDomain();

        if (!taskResult.IsSuccess)
            return taskResult.Error!;

        return taskResult.Value!;
    }

    public async System.Threading.Tasks.Task<Result<Task>> Create(Task task)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
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

        var taskRecordResult = task.FromDomain();

        if (!taskRecordResult.IsSuccess)
            return taskRecordResult.Error!;

        var result = await connection.ExecuteAsync(command, taskRecordResult.Value!);

        if (result <= 0)
        {
            throw new InvalidOperationException("Cannot insert data to database"); // TODO: Result pattern
        }

        return task;
    }

    public async System.Threading.Tasks.Task<bool> Delete(TaskId taskId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            DELETE FROM public.tasks
            WHERE task_id = @Id";

        var result = await connection.ExecuteAsync(command, new { Id = taskId.Value });

        return result > 0;
    }

    public async System.Threading.Tasks.Task<bool> Update(Task task)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            UPDATE public.tasks
            SET title = @Title
            WHERE task_id = @Id";

        var result = await connection.ExecuteAsync(command, new { Id = task.Id.Value, Title = task.Title.Value });

        return result > 0;
    }
}
