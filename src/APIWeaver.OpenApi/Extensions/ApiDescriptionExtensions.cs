using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing.Patterns;

namespace APIWeaver.OpenApi.Extensions;

internal static class ApiDescriptionExtensions
{
    public static string GetRelativePath(this ApiDescription apiDescription)
    {
        var pattern = RoutePatternFactory.Parse(apiDescription.RelativePath!);
        var segments = pattern.PathSegments.SelectMany(segment => segment.Parts).Select(part =>
        {
            return part switch
            {
                RoutePatternLiteralPart text => text.Content,
                RoutePatternParameterPart parameter => "{" + parameter.Name + "}",
                _ => throw new ArgumentException($"Unsupported route pattern part: {part}")
            };
        });

        return string.Join("/", segments);
    }
}