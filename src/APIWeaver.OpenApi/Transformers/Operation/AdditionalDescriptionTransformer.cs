using Microsoft.AspNetCore.Mvc.Controllers;

namespace APIWeaver;

/// <summary>
/// Adds additional description to the operation.
/// </summary>
internal sealed class AdditionalDescriptionTransformer : IOpenApiOperationTransformer
{
    private const string CustomNameKey = "x-name";

    /// <inheritdoc />
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // Add the name of the controller and action to the operation
        if (string.IsNullOrEmpty(operation.OperationId) && context.Description.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            var operationName = $"{controllerActionDescriptor.ControllerName}_{controllerActionDescriptor.ActionName}";
            operation.OperationId = operationName;
        }

        return Task.CompletedTask;
    }
}