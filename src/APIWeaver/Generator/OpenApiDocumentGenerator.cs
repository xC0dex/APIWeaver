using Microsoft.OpenApi.Models;

namespace APIWeaver.Generator;

internal sealed class OpenApiDocumentGenerator : IOpenApiDocumentGenerator
{
    public Task<OpenApiDocument> GenerateDocumentAsync() => Task.FromResult(new OpenApiDocument
    {
        Info = new OpenApiInfo
        {
            Title = "APIWeaver",
            Version = "1.0.0"
        },
        Paths = new OpenApiPaths
        {
            ["/"] = new()
            {
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    [OperationType.Get] = new()
                    {
                        Responses = new OpenApiResponses
                        {
                            ["200"] = new()
                            {
                                Description = "OK"
                            }
                        }
                    }
                }
            }
        }
    });
}