using APIWeaver.Extensions;
using APIWeaver.Swagger.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace APIWeaver.Swagger.Middlewares;

internal sealed class SwaggerConfigurationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Get && context.Request.Path.EndsWith("configuration.json"))
        {
            await HandleRequestAsync(context, context.RequestAborted);
            return;
        }

        await next(context);
    }

    private static Task HandleRequestAsync(HttpContext context, CancellationToken cancellationToken)
    {
        var configuration = context.RequestServices.GetRequiredService<IOptions<SwaggerUiConfiguration>>().Value;
        if (configuration.SwaggerOptions.Urls.Count == 0)
        {
            var applicationName = context.RequestServices.GetRequiredService<IWebHostEnvironment>().ApplicationName;
            configuration.SwaggerOptions.WithOpenApiEndpoint($"{applicationName} - {DocumentConstants.InitialDocumentName}", $"/{configuration.RoutePrefix}/{DocumentConstants.InitialDocumentName}-openapi.json");
        }

        return context.Response.WriteAsJsonAsync(configuration, JsonSerializerHelper.SerializerOptions, cancellationToken);
    }
}