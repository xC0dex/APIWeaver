using APIWeaver.Transformers.Schema;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace APIWeaver;

/// <summary>
/// Extension methods for adding APIWeaver services to the <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds APIWeaver services to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection" />.</param>
    /// <param name="options">Configure the <see cref="ApiWeaverOptions" />.</param>
    public static IServiceCollection AddApiWeaver(this IServiceCollection services, Action<ApiWeaverOptions> options) => services.AddApiWeaver("v1", options);

    /// <summary>
    /// Adds APIWeaver services to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection" />.</param>
    /// <param name="documentName">The name of the OpenAPI document.</param>
    /// <param name="options">Configure the <see cref="ApiWeaverOptions" />.</param>
    public static IServiceCollection AddApiWeaver(this IServiceCollection services, string documentName, Action<ApiWeaverOptions> options)
    {
        services.AddOptions<ApiWeaverOptions>(documentName).Configure(options);
        services.AddKeyedSingleton<SchemaExampleCache>(documentName);
        services.AddOptions<OpenApiOptions>(documentName).Configure<IOptionsMonitor<ApiWeaverOptions>>((openApiOptions, apiWeaverOptions) =>
        {
            var useExamples = apiWeaverOptions.Get(documentName).Examples.Count > 0;
            if (useExamples)
            {
                openApiOptions.AddSchemaTransformer<ExampleSchemaTransformer>();
            }
        });
        return services;
    }
}