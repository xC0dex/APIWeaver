using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;

namespace APIWeaver;

/// <summary>
/// Adds additional description to the operation.
/// </summary>
public sealed class AdditionalDescriptionTransformer : IOpenApiOperationTransformer
{
    private const string CustomNameKey = "x-name";

    /// <inheritdoc />
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // Add the name of the controller and action to the operation
        if (context.Description.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            if (string.IsNullOrEmpty(operation.OperationId))
            {
                var operationName = $"{controllerActionDescriptor.ControllerName}_{controllerActionDescriptor.ActionName}";
                operation.OperationId = operationName;
            }
        }

        // Add the name of the request body parameter to the request body
        if (operation.RequestBody is not null)
        {
            var requestBodyParameter = context.Description.ParameterDescriptions.FirstOrDefault(p => p.Source == BindingSource.Body);
            if (requestBodyParameter is not null)
            {
                operation.RequestBody.AddExtension(CustomNameKey, new OpenApiString(requestBodyParameter.Name));
            }
        }

        return Task.CompletedTask;
    }
}