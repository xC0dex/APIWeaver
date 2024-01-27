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
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Swagger.Middlewares;

internal sealed class SwaggerConfigurationMiddleware(RequestDelegate next)
{
    private string? _jsonResponse;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Get && context.Request.Path.EndsWith("configuration.json"))
        {
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

    private static string PrepareJsonResponse(HttpContext context)
    {
        var swaggerUiConfiguration = context.RequestServices.GetRequiredService<IOptions<SwaggerUiConfiguration>>().Value;
        var openApiConfiguration = context.RequestServices.GetRequiredService<IOptions<OpenApiOptions>>().Value;
        var swaggerUiDocumentCount = swaggerUiConfiguration.SwaggerOptions.Urls.Count;
        var openApiDocumentCount = openApiConfiguration.OpenApiDocuments.Count;

        // If manually added swagger ui documents are not equal to the number of OpenApiDocuments, throw an exception
        if (swaggerUiDocumentCount > 0 && openApiDocumentCount != swaggerUiDocumentCount)
        {
            throw new OpenApiDocumentMismatchException(openApiDocumentCount, swaggerUiDocumentCount);
        }

        // If no OpenApiDocuments are added, add an initial OpenApi and swagger ui document
        if (swaggerUiDocumentCount == 0 && openApiDocumentCount == 0)
        {
            var applicationName = context.RequestServices.GetRequiredService<IWebHostEnvironment>().ApplicationName;
            var initialApiInfo = new OpenApiInfo
            {
                Title = applicationName,
                Version = "1.0.0"
            };
            openApiConfiguration.OpenApiDocuments.Add(DocumentConstants.InitialDocumentName, initialApiInfo);
            swaggerUiConfiguration.WithOpenApiEndpoint($"{applicationName} {DocumentConstants.InitialDocumentName}", $"/{swaggerUiConfiguration.EndpointPrefix}/{DocumentConstants.InitialDocumentName}-openapi.json");
        }

        // If no swagger ui documents are added, add all to the endpoints
        else if (swaggerUiDocumentCount == 0)
        {
            foreach (var documentName in openApiConfiguration.OpenApiDocuments.Keys)
            {
                swaggerUiConfiguration.WithOpenApiDocument(documentName);
            }
        }

        return JsonSerializer.Serialize(swaggerUiConfiguration, JsonSerializerHelper.SerializerOptions);
    }
}