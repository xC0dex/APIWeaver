using APIWeaver.Schema.Models;

namespace APIWeaver;

/// <summary>
/// Represents the options for OpenAPI documents.
/// </summary>
public sealed class OpenApiOptions
{
    /// <summary>
    /// Gets or sets the collection of OpenAPI documents in the application.
    /// Each key-value pair represents a unique OpenAPI document, where the key is the document name and the value is the OpenAPI information.
    /// </summary>
    public IDictionary<string, OpenApiDocumentDefinition> OpenApiDocuments { get; set; } = new Dictionary<string, OpenApiDocumentDefinition>();

    /// <summary>
    /// Gets or sets the OpenAPI specification version. Default is <see cref="OpenApiSpecVersion.OpenApi3_0" />.
    /// </summary>
    public OpenApiSpecVersion SpecVersion { get; set; } = OpenApiSpecVersion.OpenApi3_0;

    /// <summary>
    /// Gets or sets the OpenAPI servers.
    /// </summary>
    public IList<OpenApiServer>? Servers { get; set; }

    /// <summary>
    /// Gets or sets the optional OpenAPI generator options.
    /// </summary>
    public OpenApiGeneratorOptions GeneratorOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the optional OpenAPI schema generator options.
    /// </summary>
    public OpenApiSchemaGeneratorOptions SchemaGeneratorOptions { get; set; } = new();
}