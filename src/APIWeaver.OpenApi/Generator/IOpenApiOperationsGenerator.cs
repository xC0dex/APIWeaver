using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace APIWeaver.OpenApi.Generator;

internal interface IOpenApiOperationsGenerator
{
    Task<Dictionary<OperationType, OpenApiOperation>> GenerateOperationsAsync(IEnumerable<ApiDescription> apiDescriptions, CancellationToken cancellationToken);
}