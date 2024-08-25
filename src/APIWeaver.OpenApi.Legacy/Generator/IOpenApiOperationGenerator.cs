using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace APIWeaver.OpenApi.Generator;

internal interface IOpenApiOperationGenerator
{
    OpenApiOperation GenerateOpenApiOperation(ApiDescription apiDescription);
}