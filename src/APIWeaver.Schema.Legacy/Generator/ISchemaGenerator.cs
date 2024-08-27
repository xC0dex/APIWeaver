namespace APIWeaver.Schema.Generator;

/// <summary>
/// Interface for generating <see cref="OpenApiSchema" />.
/// </summary>
public interface ISchemaGenerator
{
    /// <summary>
    /// Generates an <see cref="OpenApiSchema" /> for a given type.
    /// </summary>
    /// <param name="type">The Type for which to generate the schema.</param>
    /// <param name="customAttributes">The custom attributes for the type.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" />.</param>
    /// <returns>The OpenApiSchema for the given type.</returns>
    Task<OpenApiSchema> GenerateSchemaAsync(Type type, IEnumerable<Attribute> customAttributes, CancellationToken cancellationToken);

    /// <summary>
    /// Generates an <see cref="OpenApiSchema" /> for a given type.
    /// </summary>
    /// <param name="type">The Type for which to generate the schema.</param>
    /// <param name="customAttributes">The custom attributes for the type.</param>
    /// <returns>The OpenApiSchema for the given type.</returns>
    OpenApiSchema GenerateSchema(Type type, IEnumerable<Attribute> customAttributes);
}