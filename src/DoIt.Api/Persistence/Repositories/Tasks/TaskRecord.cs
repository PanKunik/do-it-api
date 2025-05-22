namespace DoIt.Api.Persistence.Repositories.Tasks;

// TODO: Find better naming for fields
public sealed record TaskRecord(
    Guid TaskId,
    string TaskTitle,
    DateTime TaskCreatedAt,
    bool TaskIsDone,
    bool TaskIsImportant,
    Guid? TaskListId
);