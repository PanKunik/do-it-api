namespace DoIt.Api.Persistence.Repositories.Tasks;

public sealed record TaskRecord(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsDone,
    bool IsImportant
);
