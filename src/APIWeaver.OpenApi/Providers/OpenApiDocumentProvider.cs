namespace APIWeaver.OpenApi.Providers;

internal sealed class OpenApiDocumentProvider(IOpenApiDocumentGenerator documentGenerator, IOptions<OpenApiOptions> options) : IOpenApiDocumentProvider
{
    public Task<OpenApiDocument> GetOpenApiDocumentAsync(HttpContext context, string documentName, CancellationToken cancellationToken = default)
    {
        if (!options.Value.OpenApiDocuments.TryGetValue(documentName, out var openApiDocumentDefinition))
        {
            throw new OpenApiDocumentNotFoundException(documentName);
        }

        return documentGenerator.GenerateDocumentAsync(context, openApiDocumentDefinition, cancellationToken);
    }
}