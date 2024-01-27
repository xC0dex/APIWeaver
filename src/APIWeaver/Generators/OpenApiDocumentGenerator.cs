using APIWeaver.Exceptions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Generators;

internal sealed class OpenApiDocumentGenerator(IApiDescriptionGroupCollectionProvider apiDescriptionProvider) : IOpenApiDocumentGenerator
{
    public Task<OpenApiDocument> GenerateDocumentAsync(string documentName, OpenApiInfo openApiInfo, CancellationToken cancellationToken = default)
    {
        var apiDescriptions = apiDescriptionProvider.ApiDescriptionGroups.Items.SelectMany(x => x.Items).ToArray();
        // Todo: Implement OpenApiDocument generation
        var paths = GeneratePaths(apiDescriptions);
        var document = new OpenApiDocument
        {
            Info = openApiInfo,
            Components = new OpenApiComponents(),
            Paths = paths,
            Servers = new List<OpenApiServer>(),
            SecurityRequirements = new List<OpenApiSecurityRequirement>()
        };
        return Task.FromResult(document);
    }

    private OpenApiPaths GeneratePaths(IEnumerable<ApiDescription> apiDescriptions)
    {
        var paths = new OpenApiPaths();
        foreach (var apiDescription in apiDescriptions)
        {
            var metadata = apiDescription.ActionDescriptor.EndpointMetadata;
            var operation = metadata.OfType<OpenApiOperation>().FirstOrDefault() ?? throw new OpenApiOperationNotFoundException(apiDescription.RelativePath!);
        }

        return paths;
    }
}