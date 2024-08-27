namespace APIWeaver.Schema.Repositories;

/// <summary>
/// Interface for managing OpenAPI schemas.
/// </summary>
public interface ISchemaRepository
{
    /// <summary>
    /// Retrieves a reference to an OpenAPI schema for a given type.
    /// </summary>
    /// <param name="type">The type for which to retrieve the reference.</param>
    /// <returns>The OpenAPI schema reference for the given type, or null if no schema exists.</returns>
    OpenApiSchema? GetSchemaReference(Type type);

    /// <summary>
    /// Retrieves a one of reference to an OpenAPI schema for a given type.
    /// </summary>
    /// <param name="type">The type for which to retrieve the reference.</param>
    /// <returns>The OpenAPI one of schema reference for the given type.</returns>
    OpenApiSchema GetOneOfSchemaReference(Type type);

    /// <summary>
    /// Adds a new OpenAPI schema for a given type.
    /// </summary>
    /// <param name="type">The type for which to add the schema.</param>
    /// <param name="schema">The OpenAPI schema to add.</param>
    /// <returns>A reference to the added OpenAPI schema.</returns>
    OpenApiSchema AddOpenApiSchema(Type type, OpenApiSchema schema);

    /// <summary>
    /// Retrieves all OpenAPI schemas.
    /// </summary>
    /// <returns>A dictionary of all OpenAPI schemas.</returns>
    IDictionary<string, OpenApiSchema> GetSchemas();
}