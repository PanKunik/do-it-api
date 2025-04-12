using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Domain.TaskLists;

public partial class Errors
{
    public class TaskList
    {
        public static Error IdCannotBeEmpty => Error.Validation(
            "TaskList.IdCannotBeEmpty",
            "Id of the task list cannot be empty guid."
        );

        public static Error IdCannotBeNull => Error.Validation(
            "TaskList.IdCannotBeNull",
            "Id of the task list cannot be null."
        );

        public static Error NameCannotBeNull => Error.Validation(
            "TaskList.NameCannotBeNull",
            "Name of the task list cannot be null."
        );

        public static Error NameCannotBeEmpty => Error.Validation(
            "TaskList.NameCannotBeEmpty",
            "Name of the task list cannot be empty or white space."
        );

        public static Error NameTooLong => Error.Validation(
            "TaskList.NameTooLong",
            "Name of the task list cannot be longer than 100 characters."
        );

        public static Error TasksCannotBeNull => Error.Validation(
            "TaskList.TasksCannotBeNull",
            "List of tasks cannot be null."
        );

        public static Error NotFound => Error.NotFound(
            "TaskList.NotFound",
            "Task list with specified `id` doesn't exist."
        );
    }
}