using System.Net.Mime;
using System.Text;
using System.Text.Json;
using APIWeaver.Extensions;
using APIWeaver.Models;
using APIWeaver.Swagger.Exceptions;
using APIWeaver.Swagger.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Swagger.Middleware;

internal sealed class SwaggerConfigurationMiddleware(RequestDelegate next, ILogger<SwaggerConfigurationMiddleware> logger)
{
    private string? _jsonResponse;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Get && context.Request.Path.EndsWith("configuration.json"))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Handling configuration.json request");
            }

            await HandleRequestAsync(context, context.RequestAborted);
            return;
        }

        await next(context);
    }

    private Task HandleRequestAsync(HttpContext context, CancellationToken cancellationToken)
    {
        _jsonResponse ??= PrepareJsonResponse(context);

        context.Response.Headers.ContentType = MediaTypeNames.Application.Json;
        return context.Response.WriteAsync(_jsonResponse, Encoding.UTF8, cancellationToken);
    }

    private string PrepareJsonResponse(HttpContext context)
    {
        var swaggerOptions = context.RequestServices.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        var openApiOptions = context.RequestServices.GetRequiredService<IOptions<OpenApiOptions>>().Value;
        var swaggerUiDocumentCount = swaggerOptions.UiOptions.Urls.Count;
        var openApiDocumentCount = openApiOptions.OpenApiDocuments.Count;

        // If manually added swagger ui documents are not equal to the number of OpenApiDocuments, throw an exception
        if (swaggerUiDocumentCount > 0 && openApiDocumentCount != swaggerUiDocumentCount)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("Swagger UI documents count does not match OpenAPI documents count");
            }

            throw new OpenApiDocumentMismatchException(openApiDocumentCount, swaggerUiDocumentCount);
        }

        // If no OpenApiDocuments are added, add an initial OpenApi and swagger ui document
        if (swaggerUiDocumentCount == 0 && openApiDocumentCount == 0)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("No Swagger UI or OpenAPI document provided. Creating initial OpenApiDocument");
            }

            var applicationName = context.RequestServices.GetRequiredService<IWebHostEnvironment>().ApplicationName;
            var initialApiInfo = new OpenApiInfo
            {
                Title = applicationName,
                Version = "1.0.0"
            };
            openApiOptions.OpenApiDocuments.Add(DocumentConstants.InitialDocumentName, initialApiInfo);
            swaggerOptions.WithOpenApiEndpoint($"{applicationName} {DocumentConstants.InitialDocumentName}", $"/{swaggerOptions.EndpointPrefix}/{DocumentConstants.InitialDocumentName}-openapi.json");
        }

        // If no swagger ui documents are added, add all OpenApiDocuments as swagger ui documents
        else if (swaggerUiDocumentCount == 0)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("No Swagger UI document provided. Adding OpenAPI document as Swagger UI document");
            }

            foreach (var documentName in openApiOptions.OpenApiDocuments.Keys)
            {
                swaggerOptions.WithOpenApiDocument(documentName);
            }
        }

        return JsonSerializer.Serialize(swaggerOptions, JsonSerializerHelper.SerializerOptions);
    }
}