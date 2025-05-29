namespace DoIt.Api.Persistence.Repositories.TaskLists;

public sealed record TaskListRecord(
    Guid Id,
    string Name,
    DateTime CreatedAt
);