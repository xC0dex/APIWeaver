namespace APIWeaver.OpenApi.Extensions;

internal static class HttpRequestExtensions
{
    public static string GetRequestPath(this HttpRequest request) => $"{request.Scheme}://{request.Host}{request.PathBase}";
}