using Microsoft.AspNetCore.Mvc.Controllers;

namespace APIWeaver;

/// <summary>
/// Transforms the operation ID of an <see cref="OpenApiOperation" /> to the name of the method and controller that handles the request.
/// </summary>
/// <example>
/// OperationId: <c>ControllerName</c>_<c>MethodName</c>
/// </example>
/// <remarks>
/// This transformer only works in controller based APIs.
/// </remarks>
public sealed class MethodInfoOperationTransformer : IOpenApiOperationTransformer
{
    /// <inheritdoc />
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(operation.OperationId) && context.Description.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
        {
            var methodName = actionDescriptor.MethodInfo.Name;
            var methodNameAsSpan = methodName.AsSpan();
            if (methodNameAsSpan.EndsWith("Async", StringComparison.InvariantCultureIgnoreCase))
            {
                methodName = methodNameAsSpan[..^5].ToString();
            }
            var operationId = $"{actionDescriptor.ControllerName}_{methodName}";
            operation.OperationId = operationId;
        }

        return Task.CompletedTask;
    }
}