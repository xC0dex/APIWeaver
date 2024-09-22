namespace APIWeaver;

internal sealed class ResponseTypeArrayComparer : IEqualityComparer<ResponseType[]>
{
    internal static readonly ResponseTypeArrayComparer Default = new();

    private ResponseTypeArrayComparer()
    {
    }

    public bool Equals(ResponseType[]? x, ResponseType[]? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return x.SequenceEqual(y);
    }

    public int GetHashCode(ResponseType[] obj)
    {
        return obj.Aggregate(19, (current, item) => current * 31 + item.GetHashCode());
    }
}