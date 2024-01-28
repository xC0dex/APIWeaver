using Microsoft.OpenApi.Models;

namespace APIWeaver.Generators;

/// <summary>
/// Interface for generating OpenAPI documents.
/// </summary>
public interface IOpenApiDocumentGenerator
{
    /// <summary>
    /// Asynchronously generates an OpenAPI document.
    /// </summary>
    Task<OpenApiDocument> GenerateDocumentAsync(string documentName, OpenApiInfo openApiInfo, CancellationToken cancellationToken = default);
}