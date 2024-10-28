using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver;

internal static class OpenApiOperationTransformerContextExtensions
{
    internal static async Task<bool> HasAuthorizationAsync(this OpenApiOperationTransformerContext context)
    {
        var authorizeAttributes = context.Description.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>();
        var anonymousAttributes = context.Description.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>();

        var policyProvider = context.ApplicationServices.GetService<IAuthorizationPolicyProvider>();
        var hasFallbackPolicy = await policyProvider.HasFallbackPolicyAsync();
        var isAuthorized = authorizeAttributes.Any() || hasFallbackPolicy;
        return isAuthorized && !anonymousAttributes.Any();
    }

    internal static async Task<bool> HasAnyRequirementsAsync(this OpenApiOperationTransformerContext context)
    {
        var authorizeAttributes = context.Description.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().ToArray();
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

        // If endpoint has authorize attributes but no requirements and no default policy is there, return false 
        if (authorizeAttributes.Length > 0)
        {
            return false;
        }

        // Check if the fallback policy has at least one requirement
        var fallbackPolicy = await policyProvider.GetFallbackPolicyAsync();
        return fallbackPolicy is not null && fallbackPolicy.Requirements.AnyRequirement();
    }
}