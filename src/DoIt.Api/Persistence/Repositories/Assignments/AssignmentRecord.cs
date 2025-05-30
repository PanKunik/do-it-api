namespace DoIt.Api.Persistence.Repositories.Assignments;

public sealed record AssignmentRecord(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsDone,
    bool IsImportant,
    Guid? AssignmentsListId
);