using Dapper;
using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Persistence.Database;
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

        var result = await connection.ExecuteAsync(command, taskListRecordResult.Value!);

        if (result <= 0)
            return Error.Failure("Failure", "Cannot insert `TaskList` entity to database.");

        return taskList;
    }
}