using Dapper;
using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence.Repositories.Tasks;
using DoIt.Api.Shared;
using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Persistence.Repositories.TaskLists;

public class TaskListsRepository(IDbConnectionFactory dbConnectionFactory)
    : ITaskListsRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory
        = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));

    public async Task<Result<TaskList>> Create(TaskList taskList)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            INSERT INTO public.task_lists
            (
                task_list_id
                , name
                , created_at
            )
            VALUES 
            (
                @Id
                , @Name
                , @CreatedAt
            )";

        var taskListRecordResult = taskList.FromDomain();

        if (taskListRecordResult.IsFailure)
            return taskListRecordResult.Error!;

        var result = await connection.ExecuteAsync(
            command,
            taskListRecordResult.Value!
        );

        if (result <= 0)
            return Error.Failure(
                "Failure",
                "Cannot insert `TaskList` entity to database."
            );

        return taskList;
    }

    public async Task<Result<TaskList>> GetById(TaskListId taskListId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                task_list_id AS Id
                , name AS Name
                , created_at AS CreatedAt
            FROM
                public.task_lists
            WHERE
                task_list_id = @Id;

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
                task_list_id = @Id;";

        var queryResult = await connection.QueryMultipleAsync(
            query,
            new { Id = taskListId.Value }
        );
        
        var taskListRecord = queryResult
            .Read<TaskListRecord>()
            .SingleOrDefault();
        
        var tasks =  queryResult.Read<TaskRecord>();

        if (taskListRecord is null)
            return Errors.TaskList.NotFound;
        
        taskListRecord.Tasks = tasks.ToList();

        return taskListRecord.ToDomain();
    }
}