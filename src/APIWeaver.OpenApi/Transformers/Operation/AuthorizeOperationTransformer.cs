using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Transformers;

/// <summary>
/// Transforms the operation to require authorization if the action requires it.
/// </summary>
public sealed class AuthorizeOperationTransformer : IOpenApiOperationTransformer
{
    /// <inheritdoc />
    public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var authorizeAttributes = context.Description.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().ToArray();
        var anonymousAttributes = context.Description.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>();

        var hasFallbackPolicy = await HasFallbackPolicyAsync(context);
        var isAuthorized = authorizeAttributes.Any() || hasFallbackPolicy;
        if (isAuthorized && !anonymousAttributes.Any())
        {
            AddSecurityReference(operation);
            AddResponse(operation, StatusCodes.Status401Unauthorized, "Unauthorized - A valid bearer token is required.");

            var hasRequirements = await HasRequirementsAsync(authorizeAttributes, context);
            if (hasRequirements)
            {
                AddResponse(operation, StatusCodes.Status403Forbidden, "Forbidden - The authenticated user does not have the required permissions.");
            }
        }
    }

    private static async Task<bool> HasFallbackPolicyAsync(OpenApiOperationTransformerContext context)
    {
        var policyProvider = context.ApplicationServices.GetService<IAuthorizationPolicyProvider>();
        if (policyProvider is not null)
        {
            var fallbackPolicy = await policyProvider.GetFallbackPolicyAsync();
            return fallbackPolicy is not null;
        }

        return false;
    }

    private static async Task<bool> HasRequirementsAsync(AuthorizeAttribute[] authorizeAttributes, OpenApiOperationTransformerContext context)
    {
        // Check if any authorize attribute has roles
        if (authorizeAttributes.Any(attr => !string.IsNullOrEmpty(attr.Roles)))
        {
            return true;
        }

        var policyProvider = context.ApplicationServices.GetService<IAuthorizationPolicyProvider>();
        if (policyProvider is null)
        {
            return false;
        }

        // Check if any policy defined in authorize attributes has at least one requirement
        foreach (var authorizeAttribute in authorizeAttributes)
        {
            if (!string.IsNullOrEmpty(authorizeAttribute.Policy))
            {
                var policy = await policyProvider.GetPolicyAsync(authorizeAttribute.Policy);
                if (policy is not null && policy.Requirements.AnyRequirement())
                {
                    return true;
                }
            }
        }

        // Check if the default policy has at least one requirement
        var defaultPolicy = await policyProvider.GetDefaultPolicyAsync();
        if (defaultPolicy.Requirements.AnyRequirement())
        {
            return true;
        }

        // Check if the fallback policy has at least one requirement
        var fallbackPolicy = await policyProvider.GetFallbackPolicyAsync();
        return fallbackPolicy is not null && fallbackPolicy.Requirements.AnyRequirement();
    }

    private static void AddResponse(OpenApiOperation operation, int statusCode, string description)
    {
        var statusCodeString = statusCode.ToString();
        if (!operation.Responses.ContainsKey(statusCodeString))
        {
            operation.Responses.Add(statusCodeString, new OpenApiResponse {Description = description});
        }
    }

    private static void AddSecurityReference(OpenApiOperation operation)
    {
        var schema = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };
        var requirement = new OpenApiSecurityRequirement
        {
            [schema] = []
        };
        operation.Security.Add(requirement);
    }
}