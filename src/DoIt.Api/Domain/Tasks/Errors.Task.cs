using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Tasks;

// TODO: Rethink location of this file
public partial class Errors
{
    // TODO: Rethink names of the lambda methods
    public class Task
    {
        public static Error EmptyTaskId => Error.Validation(
            "Task.EmptyTaskId",
            "Id of a task cannot be empty."
        );

        public static Error NullTaskId => Error.Validation(
            "Task.NullTaskId",
            "Task id cannot be null."
        );

        public static Error NullTitle => Error.Validation(
            "Task.NullTitle",
            "Task title cannot be null."
        );

        public static Error EmptyTitle => Error.Validation(
            "Task.EmptyTitle",
            "Task title cannot be empty."
        );

        public static Error TitleTooLong => Error.Validation(
            "Task.TitleTooLong",
            "Task title cannot exceed 100 characters."
        );

        public static Error NotFound => Error.NotFound(
            "Task.NotFound",
            "Task with specified `id` doesn't exist."
        );
    }
}
