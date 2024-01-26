using APIWeaver.Generator;
using APIWeaver.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace APIWeaver.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the APIWeaver services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    public static IServiceCollection AddApiWeaver(this IServiceCollection services)
    {
        services.TryAddSingleton<IOpenApiDocumentGenerator, OpenApiDocumentGenerator>();
        services.TryAddSingleton<OpenApiMiddleware>();
        return services;
    }
}