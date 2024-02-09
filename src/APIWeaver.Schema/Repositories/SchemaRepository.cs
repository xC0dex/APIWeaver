namespace APIWeaver.Schema.Repositories;

internal sealed class SchemaRepository : ISchemaRepository
{
    private readonly Dictionary<Type, OpenApiSchema?> _cache = new();

    public OpenApiSchema? GetSchemaReference(Type type) => !_cache.TryAdd(type, null) ? CreateSchemaReference(type) : null;

    public OpenApiSchema GetOneOfSchemaReference(Type type) =>
        new()
        {
            OneOf = [CreateSchemaReference(type)]
        };

    public OpenApiSchema AddOpenApiSchema(Type type, OpenApiSchema schema)
    {
        _cache[type] = schema;
        return CreateSchemaReference(type);
    }

    public IDictionary<string, OpenApiSchema> GetSchemas()
    {
        return _cache.Where(x => x.Value is not null).ToDictionary(key => key.Key.Name, value => value.Value!);
    }

    private static OpenApiSchema CreateSchemaReference(MemberInfo key) =>
        new()
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.Schema,
                Id = key.Name
            }
        };
}