using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;

namespace DoIt.Api.Persistence.Repositories.Assignments;

public static class Extensions
{
    public static Result<AssignmentRecord> FromDomain(this Assignment assignment)
    {
        return new AssignmentRecord(
            assignment.Id.Value,
            assignment.Title.Value,
            assignment.CreatedAt,
            assignment.IsDone,
            assignment.IsImportant,
            assignment.AssignmentsListId?.Value
        );
    }

    public static Result<Assignment> ToDomain(this AssignmentRecord assignmentRecord)
    {
        var assignmentIdResult = AssignmentId.CreateFrom(assignmentRecord.Id);

        if (assignmentIdResult.IsFailure)
            return assignmentIdResult.Error!;

        Result<AssignmentsListId>? assignmentsListIdResult = null;
        
        if (assignmentRecord.AssignmentsListId.HasValue)
        {
            assignmentsListIdResult = AssignmentsListId.CreateFrom(assignmentRecord.AssignmentsListId.Value);

            if (assignmentsListIdResult.IsFailure)
                return assignmentsListIdResult.Error!;
        }

        var titleResult = Title.CreateFrom(assignmentRecord.Title);

        if (titleResult.IsFailure)
            return titleResult.Error!;

        return Assignment.Create(
            assignmentIdResult.Value!,
            titleResult.Value!,
            assignmentRecord.CreatedAt,
            assignmentRecord.IsDone,
            assignmentRecord.IsImportant,
            assignmentsListIdResult?.Value!
        );
    }
}