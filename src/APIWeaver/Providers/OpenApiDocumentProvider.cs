using System.Collections.Concurrent;
using APIWeaver.Exceptions;
using APIWeaver.Generators;
using Microsoft.Extensions.Options;

namespace APIWeaver.Providers;

internal sealed class OpenApiDocumentProvider(IOpenApiDocumentGenerator documentGenerator, IOptions<OpenApiOptions> options) : IOpenApiDocumentProvider
{
    private readonly ConcurrentDictionary<string, OpenApiDocument> _documents = new();

    public async Task<OpenApiDocument> GetOpenApiDocumentAsync(string documentName, CancellationToken cancellationToken = default)
    {
        if (!options.Value.OpenApiDocuments.TryGetValue(documentName, out var openApiInfo))
        {
            throw new OpenApiDocumentNotFoundException(documentName);
        }

        if (!_documents.TryGetValue(documentName, out var document))
        {
            document = await documentGenerator.GenerateDocumentAsync(documentName, openApiInfo, cancellationToken);
            _documents[documentName] = document;
        }

        return document;
    }
}