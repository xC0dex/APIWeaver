using Microsoft.AspNetCore.Http;

namespace APIWeaver;

internal sealed class AuthResponseOperationTransformer : IOpenApiOperationTransformer
{
    public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var hasAuthorization = await AddUnauthorizedResponseAsync(operation, context);
        if (hasAuthorization)
        {
            await AddForbiddenResponseAsync(operation, context);
        }
    }

    private static async Task<bool> AddUnauthorizedResponseAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context)
    {
        var statusCode = StatusCodes.Status401Unauthorized.ToString();
        var containsResponse = operation.Responses.ContainsKey(statusCode);
        if (!containsResponse && await context.HasAuthorizationAsync())
        {
            operation.Responses.Add(statusCode, new OpenApiResponse { Description = "Unauthorized - Access token is missing or invalid." });
            return true;
        }

        return containsResponse;
    }

    private static async Task AddForbiddenResponseAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context)
    {
        var statusCode = StatusCodes.Status403Forbidden.ToString();
        var containsResponse = operation.Responses.ContainsKey(statusCode);
        if (!containsResponse && await context.HasAnyRequirementsAsync())
        {
            operation.Responses.Add(statusCode, new OpenApiResponse { Description = "Forbidden - The permission requirements are not met." });
        }
    }
}