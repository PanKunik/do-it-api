using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Persistence.Repositories.TaskLists;

namespace DoIt.Api.TestUtils.Builders;

public class TaskListBuilder
{
    private TaskListId Id { get; set; }
    private Name Name { get; set; }
    private DateTime CreatedAt { get; set; }
    private List<Domain.Tasks.Task> Tasks { get; set; }

    private TaskListBuilder(
        TaskListId id,
        Name name,
        DateTime createdAt,
        List<Domain.Tasks.Task> tasks
    )
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        Tasks = tasks;
    }
    
    public static TaskListBuilder Default(int no = 1)
    {
        return new TaskListBuilder(
            TaskListId.CreateFrom(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, (byte)no)).Value!,
            Name.CreateFrom($"Name {no}").Value!,
            new DateTime(2025, 1, 1).AddDays(no),
            new  List<Domain.Tasks.Task>()
        );
    }

    public TaskListBuilder WithTask(Domain.Tasks.Task task)
    {
        Tasks.Add(task);
        return this;
    }

    public TaskList Build()
    {
        return TaskList.Create(
            Id,
            Name,
            CreatedAt,
            Tasks
        ).Value!;
    }

    public async Task SaveInRepository(ITaskListsRepository taskListsRepository)
    {
        await taskListsRepository.Create(Build());
    }
}