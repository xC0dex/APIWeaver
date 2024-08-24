using APIWeaver.Swagger;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver;

/// <summary>
/// Extension methods for adding OpenAPI document services to the IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a default OpenAPI document to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services)
    {
        return services.AddOpenApiDocument(_ => { });
    }

    /// <summary>
    /// Adds an OpenAPI document to the service collection with specified options.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="options">A delegate to configure the OpenAPI options.</param>
    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services, Action<OpenApiOptions> options)
    {
        services.Configure<OpenApiHelperOptions>(x => x.Documents.Add(Constants.InitialDocumentName));
        services.AddOpenApi(options);
        return services;
    }

    /// <summary>
    /// Adds an OpenAPI document with a specified name to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="documentName">The name of the OpenAPI document.</param>
    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services, string documentName)
    {
        return services.AddOpenApiDocument(documentName, _ => { });
    }

    /// <summary>
    /// Adds an OpenAPI document with a specified name and options to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="documentName">The name of the OpenAPI document.</param>
    /// <param name="options">A delegate to configure the OpenAPI options.</param>
    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services, string documentName, Action<OpenApiOptions> options)
    {
        services.Configure<OpenApiHelperOptions>(x => x.Documents.Add(documentName));
        services.AddOpenApi(documentName, options);
        return services;
    }

    /// <summary>
    /// Adds multiple OpenAPI documents to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the OpenAPI documents to.</param>
    /// <param name="documentNames">A collection of OpenAPI document names.</param>
    public static IServiceCollection AddOpenApiDocuments(this IServiceCollection services, IEnumerable<string> documentNames)
    {
        return services.AddOpenApiDocuments(documentNames, _ => { });
    }

    /// <summary>
    /// Adds multiple OpenAPI documents with specified options to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the OpenAPI documents to.</param>
    /// <param name="documentNames">A collection of OpenAPI document names.</param>
    /// <param name="options">A delegate to configure the OpenAPI options.</param>
    public static IServiceCollection AddOpenApiDocuments(this IServiceCollection services, IEnumerable<string> documentNames, Action<OpenApiOptions> options)
    {
        foreach (var documentName in documentNames)
        {
            services.AddOpenApiDocument(documentName, options);
        }

        return services;
    }
}