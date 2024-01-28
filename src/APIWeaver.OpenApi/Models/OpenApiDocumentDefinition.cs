using Microsoft.OpenApi.Interfaces;

namespace APIWeaver.OpenApi.Models;

/// <summary>
/// Represents the definition of an OpenAPI document.
/// </summary>
public sealed class OpenApiDocumentDefinition
{
    /// <summary>
    /// Gets or sets the initial information about the API.
    /// </summary>
    public OpenApiInfo? Info { get; set; }

    /// <summary>
    /// Gets or sets the security requirements for the API.
    /// </summary>
    public IList<OpenApiSecurityRequirement>? SecurityRequirements { get; set; }


    /// <summary>
    /// Gets or sets the external documentation for the API.
    /// </summary>
    public OpenApiExternalDocs? ExternalDocs { get; set; }

    /// <summary>
    /// Gets or sets the extensions for the OpenAPI document.
    /// </summary>
    public IDictionary<string, IOpenApiExtension>? Extensions { get; set; }
}