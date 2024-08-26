using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
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
    public static IEndpointConventionBuilder MapSwaggerUi(this WebApplication app, Action<SwaggerOptions>? options = null)
    {
        var swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        options?.Invoke(swaggerOptions);
        var requestPath = $"/{swaggerOptions.RoutePrefix}";

        if (swaggerOptions.Title is null)
        {
            var hostEnvironment = app.Services.GetRequiredService<IHostEnvironment>();
            var title = $"{hostEnvironment.ApplicationName} | Swagger UI";
            swaggerOptions.Title = title;
        }
        
        // If no URLs are provided, use the OpenAPI documents registered in the options
        if (swaggerOptions.UiOptions.Urls.Count == 0)
        {
            var openApiOptions = app.Services.GetRequiredService<IOptions<OpenApiHelperOptions>>().Value;
            foreach (var document in openApiOptions.Documents)
            {
                var route = swaggerOptions.OpenApiRoutePattern.Replace("{documentName}", document);
                swaggerOptions.AddOpenApiDocument(document, route);
            }
        }

        var swaggerGroup = app.MapGroup($"{requestPath}").ExcludeFromDescription();
        swaggerGroup.MapGet("configuration.json", () => Results.Json(swaggerOptions, SwaggerOptionsSerializerContext.Default));
        swaggerGroup.MapGet("/", () => Results.Redirect($"{requestPath}/index.html"));

        app.UseStaticFiles(new StaticFileOptions
        {
            RequestPath = requestPath,
            FileProvider = new EmbeddedFileProvider(typeof(WebApplicationExtensions).Assembly, SwaggerUiAssets)
        });

        return swaggerGroup;
    }
}