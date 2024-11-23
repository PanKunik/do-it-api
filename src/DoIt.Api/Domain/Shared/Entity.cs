namespace DoIt.Api.Domain.Shared
{
    public abstract class Entity<T>
        where T : class
    {
        public T Id { get; protected set; }

        protected Entity(T id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<T> other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id.Equals(default) || other.Id.Equals(default))
                return false;

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<T> one, Entity<T> two)
        {
            if (one is null && two is null)
                return true;

            if (one is null || two is null)
                return false;

            return one.Equals(two);
        }

        public static bool operator !=(Entity<T> one, Entity<T> two)
        {
            return !(one == two);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
