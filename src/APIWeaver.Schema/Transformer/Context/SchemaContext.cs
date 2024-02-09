namespace APIWeaver.Schema.Transformer;

/// <summary>
/// Represents the context for schema transformation.
/// </summary>
/// <param name="Schema">An instance of OpenApiSchema.</param>
/// <param name="Type">The type where schema was generated for.</param>
/// <param name="Attributes">Attributes from the context.</param>
/// <param name="ServiceProvider">An instance of <see cref="IServiceProvider" />.</param>
/// <param name="CancellationToken">An instance of <see cref="CancellationToken" />.</param>
public sealed record SchemaContext(OpenApiSchema Schema, Type Type, IEnumerable<Attribute> Attributes, IServiceProvider ServiceProvider, CancellationToken CancellationToken) : TransformContext(ServiceProvider, CancellationToken);