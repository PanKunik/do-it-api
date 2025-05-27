using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Persistence.Repositories.TaskLists;

namespace DoIt.Api.TestUtils.Builders;

public class TaskListBuilder
{
    private TaskListId Id { get; set; }
    private Name Name { get; set; }
    private DateTime CreatedAt { get; set; }

    private TaskListBuilder(
        TaskListId id,
        Name name,
        DateTime createdAt
    )
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
    }
    
    public static TaskListBuilder Default(int no = 1)
    {
        return new TaskListBuilder(
            TaskListId.CreateFrom(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, (byte)no)).Value!,
            Name.CreateFrom($"Name {no}").Value!,
            new DateTime(2025, 1, 1).AddDays(no)            
        );
    }

    public TaskList Build()
    {
        return TaskList.Create(
            Id,
            Name,
            CreatedAt
        ).Value!;
    }

    public async Task SaveInRepository(ITaskListsRepository taskListsRepository)
    {
        await taskListsRepository.Create(Build());
    }
}