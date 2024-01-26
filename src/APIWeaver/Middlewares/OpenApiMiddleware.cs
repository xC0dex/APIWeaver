using System.Net.Mime;
using System.Text;
using APIWeaver.Extensions;
using APIWeaver.Generator;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;

namespace APIWeaver.Middlewares;

internal sealed class OpenApiMiddleware(IOpenApiDocumentGenerator documentGenerator) : IMiddleware
{
    private const string OpenApiSuffix = "-openapi.json";
    private const int OpenApiSuffixLength = 13;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method == HttpMethods.Get && context.Request.Path.EndsWith(OpenApiSuffix))
        {
            await HandleRequestAsync(context, context.RequestAborted);
            return;
        }

        await next(context);
    }

    private async Task HandleRequestAsync(HttpContext context, CancellationToken cancellationToken)
    {
        var path = context.Request.Path.Value!;
        var documentName = GetDocumentName(path).ToString();
        var document = await documentGenerator.GenerateDocumentAsync(documentName, cancellationToken);

        var response = context.Response;
        response.Headers.ContentType = MediaTypeNames.Application.Json;
        var jsonDocument = document.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0);
        await response.WriteAsync(jsonDocument, Encoding.UTF8, cancellationToken);
    }

    private static ReadOnlySpan<char> GetDocumentName(ReadOnlySpan<char> path)
    {
        var lastSlash = path.LastIndexOf('/');
        var lastSegment = path[(lastSlash + 1)..];
        var documentName = lastSegment[..^OpenApiSuffixLength];
        return documentName;
    }
}