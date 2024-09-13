using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace APIWeaver;

internal sealed class ClientGenerator(ILogger<ClientGenerator> logger, OpenApiDocumentProvider documentProvider)
{
    public void Generate()
    {
        var document = documentProvider.GetDocument();

        var operationsByTag = SortOperationsByTag(document);
        
        logger.LogInformation("Found {TagCount} tags in the OpenAPI document", operationsByTag.Count);
        
        foreach (var (tag, operations) in operationsByTag)
        {
            
        }
    }

    private static Dictionary<string, List<OpenApiOperation>> SortOperationsByTag(OpenApiDocument document)
    {
        var operationsByTag = new Dictionary<string, List<OpenApiOperation>>();
        foreach (var path in document.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                var tag = operation.Value.Tags.FirstOrDefault()?.Name ?? throw new InvalidOperationException("Operation must have at least one tag");
                if (!operationsByTag.TryGetValue(tag, out var value))
                {
                    value = [];
                    operationsByTag[tag] = value;
                }

                value.Add(operation.Value);
            }
        }

        return operationsByTag;
    }
}