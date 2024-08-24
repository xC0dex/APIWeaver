using APIWeaver.Swagger;
using APIWeaver.Swagger.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace APIWeaver;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder" />.
/// </summary>
public static class WebApplicationExtensions
{
    private const string SwaggerUiAssets = "SwaggerUiAssets";

    /// <summary>
    /// Configures the application to use Swagger UI.
    /// </summary>
    /// <param name="app"><see cref="WebApplication" />.</param>
    /// <param name="options">An action to configure the Swagger UI options.</param>
    public static IApplicationBuilder MapSwaggerUi(this WebApplication app, Action<SwaggerOptions>? options = null)
    {
        var swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        options?.Invoke(swaggerOptions);
        var requestPath = $"/{swaggerOptions.EndpointPrefix}";

        if (swaggerOptions.UiOptions.Urls.Count == 0)
        {
            var openApiOptions = app.Services.GetRequiredService<IOptions<InternalOpenApiOptions>>().Value;
            foreach (var document in openApiOptions.Documents)
            {
                swaggerOptions.WithOpenApiEndpoint(document, $"/openapi/{document}.json");
            }
        }
        
        var group = app.MapGroup($"{requestPath}").ExcludeFromDescription();

        group.MapGet("configuration.json", () => Results.Json(swaggerOptions, JsonSerializerHelper.SerializerOptions));
        group.MapGet("/", () => Results.Redirect($"{requestPath}/index.html"));
        app.UseStaticFiles(new StaticFileOptions
        {
            RequestPath = requestPath,
            FileProvider = new EmbeddedFileProvider(typeof(WebApplicationExtensions).Assembly, SwaggerUiAssets)
        });

        return app;
    }
}