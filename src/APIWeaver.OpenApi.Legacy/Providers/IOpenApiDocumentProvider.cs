namespace APIWeaver.OpenApi.Providers;

internal interface IOpenApiDocumentProvider
{
    Task<OpenApiDocument> GetOpenApiDocumentAsync(HttpContext context, string documentName, CancellationToken cancellationToken = default);
}