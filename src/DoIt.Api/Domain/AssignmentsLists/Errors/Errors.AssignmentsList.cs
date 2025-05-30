using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Domain;

public static partial class Errors
{
    public static class AssignmentsList
    {
        public static Error IdCannotBeEmpty => Error.Validation(
            "AssignmentsList.IdCannotBeEmpty",
            "Id of the assignments list cannot be empty guid."
        );

        public static Error NameCannotBeEmpty => Error.Validation(
            "AssignmentsList.NameCannotBeEmpty",
            "Name of the assignments list cannot be empty or white space."
        );

        public static Error NameTooLong => Error.Validation(
            "AssignmentsList.NameTooLong",
            "Name of the assignments list cannot be longer than 100 characters."
        );

        public static Error AssignmentsCannotBeNull => Error.Validation(
            "AssignmentsList.AssignmentsCannotBeNull",
            "List of assignments cannot be null."
        );

        public static Error NotFound => Error.NotFound(
            "AssignmentsList.NotFound",
            "Assignments list with specified `id` doesn't exist."
        );
    }
}