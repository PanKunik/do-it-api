using Dapper;
using DoIt.Api.Domain;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Persistence.Database;
using DoIt.Api.Shared;
using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Persistence.Repositories.Assignments;

public class AssignmentsRepository(IDbConnectionFactory dbConnectionFactory)
    : IAssignmentsRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory
        = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));

    public async Task<List<Assignment>> GetAll()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                assignment_id AS Id
                , title AS Title
                , created_at AS CreatedAt
                , is_done AS IsDone
                , is_important AS IsImportant
                , assignments_list_id AS AssignmentsListId
            FROM
                public.assignments";

        var result = await connection.QueryAsync<AssignmentRecord>(query);

        return result
            .Select(r => r.ToDomain().Value!)
            .ToList();
    }

    public async Task<Result<Assignment>> GetById(AssignmentId assignmentId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                assignment_id AS Id
                , title AS Title
                , created_at AS CreatedAt
                , is_done AS IsDone
                , is_important AS IsImportant
                , assignments_list_id AS AssignmentsListId
            FROM
                public.assignments
            WHERE
                assignment_id = @Id";

        var existingAssignment = await connection.QueryFirstOrDefaultAsync<AssignmentRecord>(
            query,
            new { Id = assignmentId.Value }
        );

        if (existingAssignment is null)
            return Errors.Assignment.NotFound;

        return existingAssignment.ToDomain();
    }

    public async Task<Result<Assignment>> Create(Assignment assignment)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            INSERT INTO public.assignments
            (
                assignment_id
                , title
                , created_at
                , is_done
                , is_important
                , assignments_list_id
            )
            VALUES
            (
                @Id
                , @Title
                , @CreatedAt
                , @IsDone
                , @IsImportant
                , @AssignmentsListId
            )";

        var assignmentRecordResult = assignment.FromDomain();

        if (assignmentRecordResult.IsFailure)
            return assignmentRecordResult.Error!;

        var result = await connection.ExecuteAsync(
            command,
            assignmentRecordResult.Value!
        );

        if (result <= 0)
            return Error.Failure(
                "Failure",
                "Cannot insert `Assignment` entity to database."
            );

        return assignment;
    }

    public async Task<Result> Delete(AssignmentId assignmentId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            DELETE FROM public.assignments
            WHERE assignment_id = @Id";

        var result = await connection.ExecuteAsync(
            command,
            new { Id = assignmentId.Value }
        );

        return result > 0 ? Result.Success() : Errors.Assignment.NotFound;
    }

    public async Task<Result> Update(Assignment assignment)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            UPDATE public.assignments
            SET
                title = @Title,
                is_done = @IsDone,
                is_important = @IsImportant,
                assignments_list_id = @AssignmentsListId
            WHERE assignment_id = @Id";

        var result = await connection.ExecuteAsync(
            command,
            new
            {
                Id = assignment.Id.Value,
                Title = assignment.Title.Value,
                assignment.IsDone,
                assignment.IsImportant,
                AssignmentsListId = assignment.AssignmentsListId?.Value
            }
        );

        return result > 0 ? Result.Success() : Errors.Assignment.NotFound;
    }
}