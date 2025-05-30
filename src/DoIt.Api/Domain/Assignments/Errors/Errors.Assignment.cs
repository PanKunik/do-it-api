using DoIt.Api.Shared.Errors;

namespace DoIt.Api.Domain;

public static partial class Errors
{
    public static class Assignment
    {
        public static Error NotFound => Error.NotFound(
            "Assignment.NotFound",
            "Assignment with specified `id` doesn't exist."
        );

        public static Error IdCannotBeEmpty => Error.Validation(
            "Assignment.IdCannotBeEmpty",
            "Id of the assignment cannot be empty guid."
        );

        public static Error TitleCannotBeEmpty => Error.Validation(
            "Assignment.TitleCannotBeEmpty",
            "Title of the assignment cannot be empty or white space."
        );

        public static Error TitleTooLong => Error.Validation(
            "Assignment.TitleTooLong",
            "Title of the assignment can have maximum of 100 characters." // TODO: How to extract this number from message?
        );
    }
}