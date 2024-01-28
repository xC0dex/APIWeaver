using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Generators;

internal sealed class OpenApiDocumentGenerator(IApiDescriptionGroupCollectionProvider apiDescriptionProvider) : IOpenApiDocumentGenerator
{
    public Task<OpenApiDocument> GenerateDocumentAsync(string documentName, OpenApiInfo openApiInfo, CancellationToken cancellationToken = default)
    {
        var document = new OpenApiDocument
        {
            Info = openApiInfo,
            Paths = new OpenApiPaths()
        };
        return Task.FromResult(document);
    }
}