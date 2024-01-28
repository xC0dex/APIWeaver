namespace APIWeaver.OpenApi.Generator;

/// <summary>
/// Interface for generating OpenAPI documents.
/// </summary>
public interface IOpenApiDocumentGenerator
{
    /// <summary>
    /// Asynchronously generates an OpenAPI document.
    /// </summary>
    /// <param name="context">The HttpContext for the current request.</param>
    /// <param name="documentDefinition">The document definition to be used in the generated document.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" />.</param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains the generated OpenApiDocument.</returns>
    Task<OpenApiDocument> GenerateDocumentAsync(HttpContext context, OpenApiDocumentDefinition documentDefinition, CancellationToken cancellationToken = default);
}