using System.Collections.Concurrent;

namespace APIWeaver;

internal sealed class ResponseCache
{
    private readonly ConcurrentBag<string[]> _cache = [];

    internal void Add(string[] value)
    {
        _cache.Add(value);
    }

    internal List<string[]> GetUniqueCombinations()
    {
        return _cache.Distinct(StringArrayComparer.Default).ToList();
    }
}