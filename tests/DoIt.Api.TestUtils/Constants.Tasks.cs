using DoIt.Api.Domain.Tasks;

namespace DoIt.Api.TestUtils;

public partial class Constants
{
    public class Tasks
    {
        public static readonly TaskId TaskId = TaskId.CreateFrom(Guid.Parse("00000000-0000-0000-0000-000000000001")).Value!;
        public static readonly Title Title = Title.CreateFrom("Task title").Value!;
        public static readonly DateTime CreatedAt = new DateTime(2012, 12, 21);

        public static readonly bool NotDone = false;
        public static readonly bool Done = true;

        public static readonly bool NotImportant = false;
        public static readonly bool Important = true;

        public static TaskId TaskIdFromIndex(int index)
            => TaskId.CreateFrom(new Guid($"00000000-0000-0000-0000-{(index + 1).ToString().PadLeft(12, '0')}")).Value!;

        public static Title TitleFromIndex(int index)
            => Title.CreateFrom($"Task title {index + 1}").Value!;

        public static DateTime CreatedAtFromIndex(int index)
            => new DateTime(2023, 1, 1).AddDays(index + 1);
    }
}
