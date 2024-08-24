using Microsoft.AspNetCore.Http;

namespace APIWeaver.Swagger.Extensions;

internal static class PathStringExtensions
{
    public static bool EndsWith(this PathString path, string segment) => path.HasValue && path.Value.EndsWith(segment, StringComparison.OrdinalIgnoreCase);
}