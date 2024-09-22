namespace APIWeaver;

internal sealed class ResponseCache
{
    private readonly List<ResponseType[]> _cache = [];

    internal void Add(ResponseType[] value)
    {
        _cache.Add(value);
    }

    internal IEnumerable<ResponseType[]> GetUniqueCombinations() => _cache.Distinct(ResponseTypeArrayComparer.Default);
}