using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddGenerator(this IServiceCollection services)
    {
        services.AddSingleton<OpenApiDocumentProvider>();
        services.AddSingleton<ClientGenerator>();
        services.AddSingleton<ResponseCache>();
        services.AddSingleton<CSharpClientProcessor>();
        return services;
    }
}