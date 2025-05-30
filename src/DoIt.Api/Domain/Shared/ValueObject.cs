namespace DoIt.Api.Domain.Shared;

public abstract class ValueObject
{
    private static bool EqualOperator(
        ValueObject left,
        ValueObject right
    )
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            return false;

        return ReferenceEquals(left, right) || left!.Equals(right);
    }

    private static bool NotEqualOperator(
        ValueObject left,
        ValueObject right
    )
    {
        return !EqualOperator(left, right);
    }

    protected abstract IEnumerable<object> GetEqualityComponent();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject) obj;

        return GetEqualityComponent()
            .SequenceEqual(other.GetEqualityComponent());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponent()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(
        ValueObject one,
        ValueObject two
    )
    {
        return EqualOperator(one, two);
    }

    public static bool operator !=(
        ValueObject one,
        ValueObject two
    )
    {
        return NotEqualOperator(one, two);
    }
}