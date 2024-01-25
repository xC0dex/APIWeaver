using Microsoft.OpenApi.Models;

namespace APIWeaver.Generator;

/// <summary>
/// Interface for generating OpenAPI documents.
/// </summary>
public interface IOpenApiDocumentGenerator
{
    /// <summary>
    /// Asynchronously generates an OpenAPI document.
    /// </summary>
    Task<OpenApiDocument> GenerateDocumentAsync();
}