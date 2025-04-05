namespace DoIt.Api.Services.TaskLists;

public sealed record TaskListDTO(
    Guid Id,
    string Name,
    DateTime CreatedAt
);