using APIWeaver.Generators;
using APIWeaver.Middleware;
using APIWeaver.Models;
using APIWeaver.Providers;
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
    public static IServiceCollection AddApiWeaver(this IServiceCollection services, Action<OpenApiOptions>? options = null)
    {
        services.AddEndpointsApiExplorer();

        services.TryAddSingleton<IOpenApiDocumentGenerator, OpenApiDocumentGenerator>();
        services.TryAddSingleton<IOpenApiDocumentProvider, OpenApiDocumentProvider>();
        services.TryAddSingleton<OpenApiMiddleware>();
        if (options is not null)
        {
            services.Configure<OpenApiOptions>(options.Invoke);
        }

        return services;
    }
}