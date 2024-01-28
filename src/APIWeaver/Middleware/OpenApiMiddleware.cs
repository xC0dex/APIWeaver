using System.Net.Mime;
using System.Text;
using APIWeaver.Extensions;
using APIWeaver.Models;
using APIWeaver.Providers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Middleware;

internal sealed class OpenApiMiddleware(IOpenApiDocumentProvider documentProvider, ILogger<OpenApiMiddleware> logger) : IMiddleware
{
    private const string OpenApiJsonSuffix = "-openapi.json";
    private const string OpenApiYamlSuffix = "-openapi.yaml";
    private const int SuffixLength = 13;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var path = context.Request.Path.Value!;
        var isJsonDocument = path.EndsWith(OpenApiJsonSuffix);
        var isYamlDocument = path.EndsWith(OpenApiYamlSuffix);
        if (context.Request.Method == HttpMethods.Get && (isJsonDocument || isYamlDocument))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Handling OpenApiDocument request for {Path}", path);
            }

            await HandleRequestAsync(context, isJsonDocument, context.RequestAborted);
            return;
        }

        await next(context);
    }

    private async Task HandleRequestAsync(HttpContext context, bool isJson, CancellationToken cancellationToken)
    {
        var options = context.RequestServices.GetRequiredService<IOptions<OpenApiOptions>>().Value;

        // Not sure if I should keep it here. Count should never be 0 when the package is used with Swagger UI.
        if (options.OpenApiDocuments.Count == 0)
        {
            var applicationName = context.RequestServices.GetRequiredService<IWebHostEnvironment>().ApplicationName;
            var initialOpenApiDocumentDefinition = new OpenApiInfo
            {
                Title = applicationName,
                Version = "1.0.0"
            };
            options.OpenApiDocuments.Add(DocumentConstants.InitialDocumentName, initialOpenApiDocumentDefinition);
        }

        var path = context.Request.Path.Value!;
        var documentName = GetDocumentName(path).ToString();
        var document = await documentProvider.GetOpenApiDocumentAsync(documentName, cancellationToken);
        var response = context.Response;
        response.Headers.ContentType = isJson ? MediaTypeNames.Application.Json : "application/x-yaml";
        if (logger.IsEnabled(LogLevel.Debug))
        {
            var format = isJson ? "JSON" : "YAML";
            logger.LogDebug("Serializing document '{Document}' into {Format} with specification version {SpecVersion}", documentName, format, options.SpecVersion);
        }

        var documentContent = isJson ? document.SerializeAsJson(options.SpecVersion) : document.SerializeAsYaml(options.SpecVersion);
        await response.WriteAsync(documentContent, Encoding.UTF8, cancellationToken);
    }

    private static ReadOnlySpan<char> GetDocumentName(ReadOnlySpan<char> path)
    {
        var lastSlash = path.LastIndexOf('/');
        var lastSegment = path[(lastSlash + 1)..];
        var documentName = lastSegment[..^SuffixLength];
        return documentName;
    }
}