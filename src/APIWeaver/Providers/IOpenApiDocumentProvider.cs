namespace APIWeaver.Providers;

internal interface IOpenApiDocumentProvider
{
    Task<OpenApiDocument> GetOpenApiDocumentAsync(string documentName, CancellationToken cancellationToken = default);
}