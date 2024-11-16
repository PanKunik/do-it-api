using DoIt.Api.Domain.Shared;

namespace DoIt.Api.Domain.Tasks
{
    public class TaskId
        : ValueObject
    {
        public Guid Value { get; private set; }

        private TaskId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Value cannot be empty.", nameof(value));

            Value = value;
        }

        public static TaskId CreateFrom(Guid value)
            => new TaskId(value);

        protected override IEnumerable<object> GetEqualityComponent()
        {
            yield return Value;
        }
    }
}
