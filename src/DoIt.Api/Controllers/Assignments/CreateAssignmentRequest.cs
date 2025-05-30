namespace DoIt.Api.Controllers.Assignments;

public sealed record CreateAssignmentRequest(
    string Title,
    bool? IsImportant,
    Guid? AssignmentsListId
);