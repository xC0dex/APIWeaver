using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace APIWeaver;

internal static class AuthorizationRequirementExtensions
{
    internal static bool AnyRequirement(this IReadOnlyList<IAuthorizationRequirement> requirements) => requirements.Any(requirement => requirement.GetType() != typeof(DenyAnonymousAuthorizationRequirement));
}