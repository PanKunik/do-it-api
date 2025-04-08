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
                @TaskListId
                , @TaskListName
                , @TaskListCreatedAt
            )";

        var taskListRecordResult = taskList.FromDomain();

        if (taskListRecordResult.IsFailure)
            return taskListRecordResult.Error!;

        var result = await connection.ExecuteAsync(command, taskListRecordResult.Value!);

        if (result <= 0)
            return Error.Failure("Failure", "Cannot insert `TaskList` entity to database.");

        return taskList;
    }

    public async Task<Result<TaskList>> GetById(TaskListId taskListId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                tl.task_list_id AS TaskListId
                , tl.name AS TaskListName
                , tl.created_at AS TaskListCreatedAt
                , t.task_id AS TaskId
                , t.title AS TaskTitle
                , t.created_at AS TaskCreatedAt
                , t.is_done AS TaskIsDone
                , t.is_important AS TaskIsImportant
            FROM
                public.task_lists tl
            LEFT JOIN
                public.tasks t
            ON
                t.task_list_id = tl.task_list_id
            WHERE
                tl.task_list_id = @Id";

        var existingTaskList = (await connection.QueryAsync<TaskListRecord, List<TaskRecord>, TaskListRecord>(
                query,
                (taskList, tasks) =>
                {
                    taskList.Tasks = tasks;
                    return taskList;
                },
                new { Id = taskListId.Value },
                splitOn: "TaskId")
            ).FirstOrDefault();

        if (existingTaskList is null)
            return Errors.TaskList.NotFound;

        return existingTaskList.ToDomain();
    }
}