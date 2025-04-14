using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Domain;

public static partial class Errors
{
    public static class TaskList
    {
        public static Error IdCannotBeEmpty => Error.Validation(
            "TaskList.IdCannotBeEmpty",
            "Id of the task list cannot be empty guid."
        );

        public static Error NameCannotBeEmpty => Error.Validation(
            "TaskList.NameCannotBeEmpty",
            "Name of the task list cannot be empty or white space."
        );

        public static Error NameTooLong => Error.Validation(
            "TaskList.NameTooLong",
            "Name of the task list cannot be longer than 100 characters."
        );
    }
}