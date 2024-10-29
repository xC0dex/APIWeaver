using Microsoft.AspNetCore.Http;

namespace APIWeaver;

internal static class HttpRequestExtensions
{
    internal static string GetRequestPath(this HttpRequest request) => $"{request.Scheme}://{request.Host}{request.PathBase}";
}