using DoIt.Api.Domain.Assignments;

namespace DoIt.Api.Services.Assignments;

public static class Extensions
{
    public static AssignmentDto ToDto(this Assignment assignment)
        => new(
            assignment.Id.Value,
            assignment.Title.Value,
            assignment.CreatedAt,
            assignment.IsDone,
            assignment.IsImportant,
            assignment.AssignmentsListId?.Value
        );
}