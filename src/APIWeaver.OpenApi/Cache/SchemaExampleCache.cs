using System.Collections.Concurrent;

namespace APIWeaver;

internal sealed class SchemaExampleCache
{
    private readonly ConcurrentDictionary<Type, IOpenApiAny> _cache = new();

    public void Add(Type type, IOpenApiAny example)
    {
        _cache[type] = example;
    }

    public IOpenApiAny? Get(Type type) => _cache.GetValueOrDefault(type);
}