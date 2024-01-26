using Microsoft.OpenApi.Models;

namespace APIWeaver.Models;

/// <summary>
/// Represents the options for OpenAPI documents.
/// </summary>
public sealed class OpenApiOptions
{

    /// <summary>
    /// Gets or sets the collection of OpenAPI documents in the application.
    /// Each key-value pair represents a unique OpenAPI document, where the key is the document name and the value is the OpenAPI information.
    /// </summary>
    public IDictionary<string, OpenApiInfo> OpenApiDocuments { get; set; } = new Dictionary<string, OpenApiInfo>();
}