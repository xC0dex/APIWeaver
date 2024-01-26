using System.Collections.Concurrent;
using APIWeaver.Exceptions;
using APIWeaver.Generators;
using APIWeaver.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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

        if (!_documents.ContainsKey(documentName))
        {
            _documents[documentName] = await documentGenerator.GenerateDocumentAsync(documentName, openApiInfo, cancellationToken);
        }

        return _documents[documentName];
    }
}