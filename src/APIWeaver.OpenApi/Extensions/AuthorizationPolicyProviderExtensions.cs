namespace APIWeaver;

internal static class AuthorizationPolicyProviderExtensions
{
    internal static async Task<bool> HasFallbackPolicyAsync(this IAuthorizationPolicyProvider? policyProvider)
    {
        if (policyProvider is null)
        {
            return false;
        }

        var fallbackPolicy = await policyProvider.GetFallbackPolicyAsync();
        return fallbackPolicy is not null;
    }
}