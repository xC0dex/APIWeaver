namespace APIWeaver.Schema.Models;

/// <summary>
/// Options for generating an OpenAPI schema.
/// </summary>
public sealed class OpenApiSchemaGeneratorOptions
{
    /// <summary>
    /// Gets or sets the source of the JSON options.
    /// Default is <see cref="JsonOptionsSource.MinimalApiOptions" />.
    /// </summary>
    public JsonOptionsSource JsonOptionsSource { get; set; } = JsonOptionsSource.MinimalApiOptions;

    /// <summary>
    /// Indicates if the '?' nullable operator should be recognized for nullability checks on references types.
    /// </summary>
    public bool NullableAnnotationForReferenceTypes { get; set; } = true;

    /// <summary>
    /// A list of <see cref="ISchemaTransformer" /> instances.
    /// Each instance can be used to transform an <see cref="OpenApiSchema" />.
    /// </summary>
    public IList<ITransformer<SchemaContext>> SchemaTransformers { get; } = [];
}