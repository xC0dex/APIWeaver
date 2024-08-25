using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace APIWeaver.OpenApi.Generator;

internal interface IOpenApiOperationsGenerator
{
    Task<Dictionary<OperationType, OpenApiOperation>> GenerateOpenApiOperationsAsync(IEnumerable<ApiDescription> apiDescriptions, CancellationToken cancellationToken);
}