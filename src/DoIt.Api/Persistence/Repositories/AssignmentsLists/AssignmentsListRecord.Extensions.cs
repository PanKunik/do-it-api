using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;

namespace DoIt.Api.Persistence.Repositories.AssignmentsLists;

public static class Extensions
{
    public static Result<AssignmentsListRecord> FromDomain(this AssignmentsList assignmentsList)
    {
        var assignmentsListRecord = new AssignmentsListRecord(
            assignmentsList.Id.Value,
            assignmentsList.Name.Value,
            assignmentsList.CreatedAt
        );

        return assignmentsListRecord;
    }

    public static Result<AssignmentsList> ToDomain(
        this AssignmentsListRecord assignmentsListRecord,
        List<Assignment>? assignments = null
    )
    {
        var assignmentsListIdResult = AssignmentsListId.CreateFrom(assignmentsListRecord.Id);
        
        if (assignmentsListIdResult.IsFailure)
            return assignmentsListIdResult.Error!;
        
        var assignmentsListNameResult = Name.CreateFrom(assignmentsListRecord.Name);
        
        if (assignmentsListNameResult.IsFailure)
            return assignmentsListNameResult.Error!;

        return AssignmentsList.Create(
            assignmentsListIdResult.Value!,
            assignmentsListNameResult.Value!,
            assignmentsListRecord.CreatedAt,
            assignments
        );
    }
}