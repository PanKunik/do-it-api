using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Tasks;

public partial class Errors
{
    public class Task
    {
        public static Error NotFound => Error.NotFound(
            "Task.NotFound",
            "Task with specified `id` doesn't exist."
        );
    }
}
