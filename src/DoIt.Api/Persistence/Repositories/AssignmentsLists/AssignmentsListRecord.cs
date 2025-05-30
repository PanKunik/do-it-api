namespace DoIt.Api.Persistence.Repositories.AssignmentsLists;

public sealed record AssignmentsListRecord(
    Guid Id,
    string Name,
    DateTime CreatedAt
);