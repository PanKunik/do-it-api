using DoIt.Api.Domain.Assignments;

namespace DoIt.Api.TestUtils;

[Obsolete("Move to builders")]
public partial class Constants
{
    public class Tasks
    {
        public static readonly AssignmentId AssignmentId = AssignmentId.CreateFrom(Guid.Parse("00000000-0000-0000-0000-000000000001")).Value!;
        public static readonly Title Title = Title.CreateFrom("Task title").Value!;
        public static readonly DateTime CreatedAt = new DateTime(2012, 12, 21);

        public const bool NotDone = false;
        public const bool Done = true;

        public const bool NotImportant = false;
        public const bool Important = true;

        public static AssignmentId TaskIdFromIndex(int index)
            => AssignmentId.CreateFrom(new Guid($"00000000-0000-0000-0000-{(index + 1).ToString().PadLeft(12, '0')}")).Value!;

        public static Title TitleFromIndex(int index)
            => Title.CreateFrom($"Task title {index + 1}").Value!;

        public static DateTime CreatedAtFromIndex(int index)
            => new DateTime(2023, 1, 1).AddDays(index + 1);
    }
}
