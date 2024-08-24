using APIWeaver.Swagger;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services)
    {
        return services.AddOpenApiDocument(_ => { });
    }

    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services, Action<OpenApiOptions> options)
    {
        services.Configure<InternalOpenApiOptions>(x => x.Documents.Add(Constants.InitialDocumentName));
        services.AddOpenApi(options);
        return services;
    }

    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services, string documentName)
    {
        return services.AddOpenApiDocument(documentName, _ => { });
    }

    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services, string documentName, Action<OpenApiOptions> options)
    {
        services.Configure<InternalOpenApiOptions>(x => x.Documents.Add(documentName));
        services.AddOpenApi(documentName, options);
        return services;
    }
}