namespace DoIt.Api.Services.Tasks;

public sealed record TaskDto(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsDone,
    bool IsImportant,
    Guid? TaskListId
);