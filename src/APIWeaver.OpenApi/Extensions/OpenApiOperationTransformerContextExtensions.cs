using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
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
}