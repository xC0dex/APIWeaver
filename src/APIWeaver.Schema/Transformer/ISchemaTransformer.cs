namespace APIWeaver.Schema.Transformer;

/// <summary>
/// Represents a interface that transforms an <see cref="OpenApiSchema" />.
/// </summary>
public interface ISchemaTransformer : ITransformer<SchemaContext>;