namespace DoIt.Api.Services.Tasks;

public sealed record TaskDTO(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsDone,
    bool IsImportant
);
