using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Repositories.Tasks;

namespace DoIt.Api.TestUtils.Builders;

public class TaskBuilder
{
    private TaskId Id { get; set; }
    private Title Title { get; set; }
    private DateTime CreatedAt  { get; set; }
    private bool IsDone  { get; set; }
    private bool IsImportant  { get; set; }
    private TaskListId? TaskListId { get; set; }

    private TaskBuilder(
        TaskId id,
        Title title,
        DateTime createdAt,
        bool? isDone = null,
        bool? isImportant = null,
        TaskListId? taskListId = null
    )
    {
        Id = id;
        Title = title;
        CreatedAt = createdAt;
        IsDone = isDone ?? false;
        IsImportant = isImportant ?? false;
        TaskListId = taskListId;
    }
    
    public static TaskBuilder Default(int no = 1)
    {
        return new TaskBuilder(
            TaskId.CreateFrom(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, (byte)no)).Value!,
            Title.CreateFrom($"Title {no}").Value!,
            new DateTime(2025, 1, 1).AddDays(no),
            false,
            false
        );
    }

    public TaskBuilder WithId(Guid id)
    {
        Id = TaskId.CreateFrom(id).Value!;
        return this;
    }

    public TaskBuilder WithTaskListId(Guid taskListId)
    {
        TaskListId = TaskListId.CreateFrom(taskListId).Value!;
        return this;
    }

    public Domain.Tasks.Task Build()
    {
        return Domain.Tasks.Task.Create(
            Id,
            Title,
            CreatedAt,
            IsDone,
            IsImportant,
            TaskListId
        ).Value!;
    }

    public async System.Threading.Tasks.Task SaveInRepository(ITasksRepository tasksRepository)
    {
        await tasksRepository.Create(Build());
    }
}