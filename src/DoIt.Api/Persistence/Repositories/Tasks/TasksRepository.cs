using Dapper;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Database;
using DoIt.Api.Shared;
using DoIt.Api.Shared.Errors;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Persistence.Repositories.Tasks;

public class TasksRepository(IDbConnectionFactory dbConnectionFactory)
    : ITasksRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory
        = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));

    public async System.Threading.Tasks.Task<List<Task>> GetAll()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                task_id AS Id
                , title AS Title
                , created_at AS CreatedAt
                , is_done AS IsDone
                , is_important AS IsImportant
                , task_list_id AS TaskListId
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
                , title AS Title
                , created_at AS CreatedAt
                , is_done AS IsDone
                , is_important AS IsImportant
                , task_list_id AS TaskListId
            FROM
                public.tasks
            WHERE
                task_id = @Id";

        var existingTask = await connection.QueryFirstOrDefaultAsync<TaskRecord>(
            query,
            new { Id = taskId.Value }
        );

        if (existingTask is null)
            return Errors.Task.NotFound;

        return existingTask.ToDomain();
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
                , task_list_id
            )
            VALUES
            (
                @Id
                , @Title
                , @CreatedAt
                , @IsDone
                , @IsImportant
                , @TaskListId
            )";

        var taskRecordResult = task.FromDomain();

        if (taskRecordResult.IsFailure)
            return taskRecordResult.Error!;

        var result = await connection.ExecuteAsync(
            command,
            taskRecordResult.Value!
        );

        if (result <= 0)
            return Error.Failure(
                "Failure",
                "Cannot insert `Task` entity to database."
            );

        return task;
    }

    public async System.Threading.Tasks.Task<Result> Delete(TaskId taskId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            DELETE FROM public.tasks
            WHERE task_id = @Id";

        var result = await connection.ExecuteAsync(
            command,
            new { Id = taskId.Value }
        );

        return result > 0 ? Result.Success() : Errors.Task.NotFound;
    }

    public async System.Threading.Tasks.Task<Result> Update(Task task)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            UPDATE public.tasks
            SET
                title = @Title,
                is_done = @IsDone,
                is_important = @IsImportant
            WHERE task_id = @Id";

        var result = await connection.ExecuteAsync(
            command,
            new
            {
                Id = task.Id.Value,
                Title = task.Title.Value,
                task.IsDone,
                task.IsImportant
            }
        );

        return result > 0 ? Result.Success() : Errors.Task.NotFound;
    }
}