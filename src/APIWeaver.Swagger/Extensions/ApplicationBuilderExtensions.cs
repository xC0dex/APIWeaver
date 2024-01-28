using APIWeaver.Middleware;
using APIWeaver.Swagger.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace APIWeaver.Swagger.Extensions;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder" />.
/// </summary>
public static class ApplicationBuilderExtensions
{
    private const string SwaggerUiAssets = "SwaggerUiAssets";

    /// <summary>
    /// Configures the application to use Swagger UI.
    /// </summary>
    /// <param name="appBuilder"><see cref="IApplicationBuilder" />.</param>
    /// <param name="options">An action to configure the Swagger UI options.</param>
    public static IApplicationBuilder UseSwaggerUi(this IApplicationBuilder appBuilder, Action<SwaggerOptions>? options = null)
    {
        var swaggerOptions = appBuilder.ApplicationServices.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        options?.Invoke(swaggerOptions);
        var requestPath = $"/{swaggerOptions.EndpointPrefix}";

        appBuilder.MapWhen(context => context.Request.Path.StartsWithSegments(requestPath), builder =>
        {
            builder.UseMiddleware<OpenApiMiddleware>();
            builder.UseMiddleware<SwaggerConfigurationMiddleware>();

            builder.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = requestPath,
                FileProvider = new EmbeddedFileProvider(typeof(SwaggerConfigurationMiddleware).Assembly, SwaggerUiAssets)
            });

            builder.Use((context, next) =>
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.Redirect($"{requestPath}/index.html");
                    return Task.CompletedTask;
                }

                return next(context);
            });
        });

        return appBuilder;
    }
}