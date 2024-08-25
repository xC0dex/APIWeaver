using APIWeaver.Core;
using APIWeaver.OpenApi.Middleware;
using Microsoft.AspNetCore.Builder;

namespace APIWeaver;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder" />.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the OpenApi middleware to the specified application builder with the provided endpoint prefix.
    /// This extension method only needs to be called when no swagger ui is used.
    /// </summary>
    /// <param name="appBuilder"><see cref="IApplicationBuilder" />.</param>
    /// <param name="endpointPrefix">The prefix for the OpenApi endpoints. Default is <see cref="Constants.DefaultEndpointPrefix" />.</param>
    public static IApplicationBuilder UseOpenApi(this IApplicationBuilder appBuilder, string endpointPrefix = Constants.DefaultEndpointPrefix)
    {
        var requestPath = $"/{endpointPrefix.Trim('/')}";
        return appBuilder.MapWhen(context => context.Request.Path.StartsWithSegments(requestPath), builder => { builder.UseMiddleware<OpenApiMiddleware>(); });
    }
}