using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Domain.Tasks;

public partial class Errors
{
    public class Task
    {
        public static Error NotFound => Error.NotFound(
            "Task.NotFound",
            "Task with specified `id` doesn't exist."
        );

        public static Error IdCannotBeNull => Error.Validation(
            "Task.IdCannotBeNull",
            "Id of the task cannot be null."
        );

        public static Error IdCannotBeEmpty => Error.Validation(
            "Task.IdCannotBeEmpty",
            "Id of the task cannot be empty guid."
        );

        public static Error TitleCannotBeNull => Error.Validation(
            "Task.TitleCannotBeNull",
            "Title of the task cannot be null."
        );

        public static Error TitleCannotBeEmpty => Error.Validation(
            "Task.TitleCannotBeEmpty",
            "Title of the task cannot be empty or white space."
        );

        public static Error TitleTooLong => Error.Validation(
            "Task.TitleTooLong",
            "Title of the task can have maximum of 100 characters." // TODO: How to extract this number from message?
        );
    }
}