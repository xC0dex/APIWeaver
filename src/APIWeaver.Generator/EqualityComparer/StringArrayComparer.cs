namespace APIWeaver;

internal sealed class StringArrayComparer : IEqualityComparer<string[]>
{
    internal static readonly StringArrayComparer Default = new();

    private StringArrayComparer()
    {
    }

    public bool Equals(string[]? x, string[]? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return x.SequenceEqual(y);
    }

    public int GetHashCode(string[] obj)
    {
        return obj.Aggregate(19, (current, item) => current * 31 + item.GetHashCode());
    }
}