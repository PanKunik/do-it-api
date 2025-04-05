using DoIt.Api.Domain.TaskLists;

namespace DoIt.Api.TestUtils;

public partial class Constants
{
    public class TaskLists
    {
        public static readonly TaskListId TaskListId = TaskListId.CreateFrom(Guid.Parse("00000000-0000-0000-0000-000000000001")).Value!;
        public static readonly Name Name = Name.CreateFrom("Name").Value!;
        public static readonly DateTime CreatedAt = new DateTime(2020, 1, 1);
        
        public static TaskListId TaskListIdFromIndex(int index)
            => TaskListId.CreateFrom(new Guid($"00000000-0000-0000-0000-{(index + 1).ToString().PadLeft(12, '0')}")).Value!;

        public static Name NameFromIndex(int index)
            => Name.CreateFrom($"Name {index + 1}").Value!;
        
        public static DateTime CreatedAtFromIndex(int index)
            => new DateTime(2020, 1, 1).AddDays(index + 1);
    }
}