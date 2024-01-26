using APIWeaver.Generators;
using APIWeaver.Middlewares;
using APIWeaver.Models;
using APIWeaver.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

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
        services.AddEndpointsApiExplorer();

        services.TryAddSingleton<IOpenApiDocumentGenerator, OpenApiDocumentGenerator>();
        services.TryAddSingleton<IOpenApiDocumentProvider, OpenApiDocumentProvider>();
        services.TryAddSingleton<OpenApiMiddleware>();

        services.Configure<OpenApiOptions>(x => x.OpenApiDocuments.Add(DocumentConstants.InitialDocumentName, new OpenApiInfo
        {
            Title = "TODO",
            Version = "1.0.0"
        }));
        return services;
    }
}