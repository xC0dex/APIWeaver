using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace APIWeaver.OpenApi.Generator;

internal sealed class OpenApiOperationsGenerator(
    ISchemaGenerator schemaGenerator,
    IOpenApiOperationGenerator operationGenerator,
    IOptions<OpenApiOptions> options,
    IServiceProvider serviceProvider) : IOpenApiOperationsGenerator
{
    public async Task<Dictionary<OperationType, OpenApiOperation>> GenerateOpenApiOperationsAsync(IEnumerable<ApiDescription> apiDescriptions, CancellationToken cancellationToken)
    {
        var apiDescriptionsByMethod = apiDescriptions.GroupBy(apiDescription => apiDescription.HttpMethod.ToOperationType());
        var operations = new Dictionary<OperationType, OpenApiOperation>();
        foreach (var group in apiDescriptionsByMethod)
        {
            var method = group.Key;
            if (group.Count() > 1)
            {
                throw new OpenApiOperationConflictException(method);
            }

            var operation = await GenerateOpenApiOperationAsync(group.First(), cancellationToken);
            operations.Add(method, operation);
        }

        return operations;
    }

    private async Task<OpenApiOperation> GenerateOpenApiOperationAsync(ApiDescription apiDescription, CancellationToken cancellationToken)
    {
        var metadata = apiDescription.ActionDescriptor.EndpointMetadata;
        var operation = metadata.OfType<OpenApiOperation>().FirstOrDefault() ?? operationGenerator.GenerateOpenApiOperation(apiDescription);

        foreach (var parameter in operation.Parameters)
        {
            var parameterDescription = apiDescription.ParameterDescriptions.GetPathParameters().FirstOrDefault(x => x.Name == parameter.Name);
            if (parameterDescription is not null)
            {
                var parameterInfo = parameterDescription.GetParameterInfo();
                parameter.Schema = await schemaGenerator.GenerateSchemaAsync(parameterInfo.ParameterType, parameterInfo.GetCustomAttributes(), cancellationToken);
            }
        }

        var requestContentType = operation.RequestBody?.Content?.Values.FirstOrDefault();
        if (requestContentType is not null)
        {
            var parameterDescription = apiDescription.ParameterDescriptions.GetRequestBodyParameters().FirstOrDefault();
            if (parameterDescription is not null)
            {
                var parameterInfo = parameterDescription.GetParameterInfo();
                requestContentType.Schema = await schemaGenerator.GenerateSchemaAsync(parameterInfo.ParameterType, parameterInfo.GetCustomAttributes(), cancellationToken);
            }
        }

        foreach (var (statusCode, openApiResponse) in operation.Responses)
        {
            var responseType = apiDescription.SupportedResponseTypes.FirstOrDefault(desc => desc.StatusCode.ToString() == statusCode);
            if (responseType is not null)
            {
                var responseSchema = await schemaGenerator.GenerateSchemaAsync(responseType.Type!, [], cancellationToken);
                var responseContentTypes = openApiResponse?.Content?.Values;
                if (responseContentTypes is not null)
                {
                    foreach (var responseContentType in responseContentTypes)
                    {
                        responseContentType.Schema = responseSchema;
                    }
                }
            }
        }

        var operationContext = new OperationContext(operation, serviceProvider, cancellationToken);
        foreach (var operationTransformer in options.Value.GeneratorOptions.OperationTransformers)
        {
            var task = operationTransformer.TransformAsync(operationContext);
            if (!task.IsCompleted)
            {
                await task;
            }
        }

        return operation;
    }
}