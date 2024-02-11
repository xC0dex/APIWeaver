using APIWeaver.OpenApi.Middleware;
using APIWeaver.Schema.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace APIWeaver;

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

        services.AddApiWeaverSchema();

        services.TryAddScoped<IOpenApiDocumentGenerator, OpenApiDocumentGenerator>();
        services.TryAddScoped<IOpenApiOperationsGenerator, OpenApiOperationsGenerator>();
        services.TryAddScoped<IOpenApiServersGenerator, OpenApiServersGenerator>();
        services.TryAddScoped<IOpenApiDocumentProvider, OpenApiDocumentProvider>();
        services.TryAddScoped<OpenApiMiddleware>();
        if (options is not null)
        {
            services.Configure<OpenApiOptions>(options.Invoke);
        }

        services.TryAddSingleton(provider =>
        {
            var openApiOptions = provider.GetRequiredService<IOptions<OpenApiOptions>>();
            return Options.Create(openApiOptions.Value.SchemaGeneratorOptions);
        });

        return services;
    }
}