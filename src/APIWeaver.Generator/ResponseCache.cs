using System.Collections.Concurrent;

namespace APIWeaver;

internal sealed class ResponseCache
{
    private readonly ConcurrentBag<ResponseType[]> _cache = [];

    internal void Add(ResponseType[] value)
    {
        _cache.Add(value);
    }

    internal List<ResponseType[]> GetUniqueCombinations() => _cache.Distinct(ResponseTypeArrayComparer.Default).ToList();
}