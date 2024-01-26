using System.Net.Mime;
using System.Text;
using APIWeaver.Generator;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;

namespace APIWeaver.Middlewares;

internal sealed class OpenApiMiddleware(IOpenApiDocumentGenerator documentGenerator) : IMiddleware
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
            await HandleRequestAsync(context, isJsonDocument, context.RequestAborted);
            return;
        }

        await next(context);
    }

    private async Task HandleRequestAsync(HttpContext context, bool isJson, CancellationToken cancellationToken)
    {
        var path = context.Request.Path.Value!;
        var documentName = GetDocumentName(path).ToString();
        var document = await documentGenerator.GenerateDocumentAsync(documentName, cancellationToken);
        var response = context.Response;
        response.Headers.ContentType = isJson ? MediaTypeNames.Application.Json : "application/x-yaml";
        var documentContent = isJson ? document.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0) : document.SerializeAsYaml(OpenApiSpecVersion.OpenApi3_0);
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