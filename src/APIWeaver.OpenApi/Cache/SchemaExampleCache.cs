namespace APIWeaver;

internal sealed class SchemaExampleCache
{
    private readonly Dictionary<object, IOpenApiAny> _cache = new();

    private readonly Lock _lock = new();

    public IOpenApiAny? GetOrAdd(object example, Func<IOpenApiAny?> factory)
    {
        lock (_lock)
        {
            if (_cache.TryGetValue(example, out var openApiExample))
            {
                return openApiExample;
            }

            openApiExample = factory();
            if (openApiExample is not null)
            {
                _cache[example] = openApiExample;
            }

            return openApiExample;
        }
    }
}