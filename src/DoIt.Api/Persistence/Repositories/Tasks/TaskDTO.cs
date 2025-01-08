namespace DoIt.Api.Persistence.Repositories;

public sealed class TaskDTO
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDone { get; set; }
    public bool IsImportant { get; set; }
}
