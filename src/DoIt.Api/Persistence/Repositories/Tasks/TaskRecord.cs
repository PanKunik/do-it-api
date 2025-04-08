namespace DoIt.Api.Persistence.Repositories.Tasks;

public sealed record TaskRecord(
    Guid TaskId,
    string TaskTitle,
    DateTime TaskCreatedAt,
    bool TaskIsDone,
    bool TaskIsImportant,
    Guid? TaskListId
);