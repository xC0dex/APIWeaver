using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace APIWeaver.OpenApi.Generator;

internal sealed class OpenApiDocumentGenerator(
    IApiDescriptionGroupCollectionProvider apiDescriptionProvider,
    IServiceProvider serviceProvider,
    IOptions<OpenApiOptions> options,
    IOpenApiOperationsGenerator operationsGenerator,
    IOpenApiServersGenerator serversGenerator,
    ISchemaRepository schemaRepository)
    : IOpenApiDocumentGenerator
{
    public async Task<OpenApiDocument> GenerateDocumentAsync(HttpContext context, OpenApiDocumentDefinition documentDefinition, CancellationToken cancellationToken = default)
    {
        var generatePathsTask = GeneratePathsAsync(cancellationToken);
        var generateServersTask = serversGenerator.GenerateServersAsync(context.Request.GetRequestPath(), cancellationToken);
        await Task.WhenAll(generatePathsTask, generateServersTask);
        var document = new OpenApiDocument
        {
            Info = documentDefinition.Info,
            Paths = generatePathsTask.Result,
            Servers = generateServersTask.Result,
            SecurityRequirements = documentDefinition.SecurityRequirements,
            ExternalDocs = documentDefinition.ExternalDocs,
            Extensions = documentDefinition.Extensions,
            Components = new OpenApiComponents
            {
                Schemas = schemaRepository.GetSchemas()
            }
        };
        var documentContext = new DocumentContext(document, serviceProvider, cancellationToken);
        foreach (var documentTransformer in options.Value.GeneratorOptions.DocumentTransformers)
        {
            var task = documentTransformer.TransformAsync(documentContext);
            if (!task.IsCompleted)
            {
                await task;
            }
        }

        return document;
    }

    private async Task<OpenApiPaths> GeneratePathsAsync(CancellationToken cancellationToken)
    {
        var paths = new OpenApiPaths();
        var apiDescriptions = apiDescriptionProvider.ApiDescriptionGroups.Items.SelectMany(x => x.Items).ToArray();
        var apiDescriptionsByPath = apiDescriptions.GroupBy(apiDescription => apiDescription.GetRelativePath());
        foreach (var group in apiDescriptionsByPath)
        {
            var path = group.Key;
            var operations = await operationsGenerator.GenerateOperationsAsync(group, cancellationToken);
            var item = new OpenApiPathItem
            {
                Operations = operations
            };
            paths.Add($"/{path}", item);
        }

        return paths;
    }
}