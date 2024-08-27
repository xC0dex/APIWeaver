using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace APIWeaver;

/// <summary>
/// Extension methods for <see cref="IEndpointRouteBuilder" />.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    private const string SwaggerUiAssets = "SwaggerUiAssets";

    /// <summary>
    /// Configures the application to use Swagger UI.
    /// </summary>
    /// <param name="builder"><see cref="IEndpointRouteBuilder" />.</param>
    /// <param name="options">An action to configure the Swagger UI options.</param>
    public static IEndpointConventionBuilder MapSwaggerUi(this IEndpointRouteBuilder builder, Action<SwaggerOptions>? options = null)
    {
        var swaggerOptions = builder.ServiceProvider.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        options?.Invoke(swaggerOptions);
        var requestPath = $"/{swaggerOptions.RoutePrefix}";

        if (swaggerOptions.Title is null)
        {
            var hostEnvironment = builder.ServiceProvider.GetRequiredService<IHostEnvironment>();
            var title = $"{hostEnvironment.ApplicationName} | Swagger UI";
            swaggerOptions.Title = title;
        }

        // If no URLs are provided, use the OpenAPI documents registered in the options
        if (swaggerOptions.Urls.Count == 0)
        {
            var openApiOptions = builder.ServiceProvider.GetRequiredService<IOptions<OpenApiHelperOptions>>().Value;
            foreach (var document in openApiOptions.Documents)
            {
                var route = swaggerOptions.OpenApiRoutePattern.Replace("{documentName}", document);
                swaggerOptions.AddOpenApiDocument(document, route);
            }
        }

        var fileProvider = new EmbeddedFileProvider(typeof(EndpointRouteBuilderExtensions).Assembly, SwaggerUiAssets);
        var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

        var swaggerGroup = builder.MapGroup($"{requestPath}").ExcludeFromDescription();
        swaggerGroup.MapGet("configuration.json", () => Results.Json(swaggerOptions, SwaggerOptionsSerializerContext.Default));
        swaggerGroup.MapGet("/", () => Results.Redirect($"{requestPath}/index.html"));
        swaggerGroup.MapGet("{**path}", (string path) =>
        {
            var file = fileProvider.GetFileInfo(path);
            if (file.Exists)
            {
                var contentType = fileExtensionContentTypeProvider.TryGetContentType(path, out var type) ? type : "application/octet-stream";
                return Results.Stream(file.CreateReadStream(), contentType);
            }

            return Results.NotFound();
        });

        return swaggerGroup;
    }
}