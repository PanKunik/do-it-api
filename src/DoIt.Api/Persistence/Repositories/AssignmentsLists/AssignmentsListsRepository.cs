using Dapper;
using DoIt.Api.Domain;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Shared;
using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Persistence.Repositories.AssignmentsLists;

public class AssignmentsListsRepository(IDbConnectionFactory dbConnectionFactory)
    : IAssignmentsListsRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory
        = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));

    public async Task<List<AssignmentsList>> GetAll()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                assignments_list_id AS Id,
                name AS Name,
                created_at AS CreatedAt
            FROM
                public.assignments_lists";
        
        var result = await connection.QueryAsync<AssignmentsListRecord>(query);
        
        return result
            .Select(r => r.ToDomain().Value!)
            .ToList();
    }
    
    public async Task<Result<AssignmentsList>> Create(AssignmentsList assignmentsList)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var command = @"
            INSERT INTO public.assignments_lists
            (
                assignments_list_id
                , name
                , created_at
            )
            VALUES 
            (
                @Id
                , @Name
                , @CreatedAt
            )";

        var assignmentsListRecordResult = assignmentsList.FromDomain();

        if (assignmentsListRecordResult.IsFailure)
            return assignmentsListRecordResult.Error!;

        var result = await connection.ExecuteAsync(
            command,
            assignmentsListRecordResult.Value!
        );

        if (result <= 0)
            return Error.Failure(
                "Failure",
                "Cannot insert `AssignmentsList` entity to database."
            );

        return assignmentsList;
    }

    public async Task<Result<AssignmentsList>> GetById(AssignmentsListId assignmentsListId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var query = @"
            SELECT
                assignments_list_id AS Id
                , name AS Name
                , created_at AS CreatedAt
            FROM
                public.assignments_lists
            WHERE
                assignments_list_id = @Id;

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
                assignments_list_id = @Id;";

        var queryResult = await connection.QueryMultipleAsync(
            query,
            new { Id = assignmentsListId.Value }
        );
        
        var assignmentsListRecord = queryResult
            .Read<AssignmentsListRecord>()
            .SingleOrDefault();
        
        if (assignmentsListRecord is null)
            return Errors.AssignmentsList.NotFound;
        
        var assignmentsRecords =  queryResult.Read<AssignmentRecord>();

        var assignments = assignmentsRecords
            .Select(t => t.ToDomain().Value!)
            .ToList();
        
        return assignmentsListRecord.ToDomain(assignments);
    }
}