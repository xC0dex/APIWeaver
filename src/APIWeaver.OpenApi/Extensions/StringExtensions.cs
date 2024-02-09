namespace APIWeaver.OpenApi.Extensions;

internal static class StringExtensions
{
    public static OperationType ToOperationType(this string? httpMethod)
    {
        return httpMethod switch
        {
            "GET" => OperationType.Get,
            "PUT" => OperationType.Put,
            "POST" => OperationType.Post,
            "DELETE" => OperationType.Delete,
            "OPTIONS" => OperationType.Options,
            "HEAD" => OperationType.Head,
            "PATCH" => OperationType.Patch,
            "TRACE" => OperationType.Trace,
            _ => throw new ArgumentException($"Invalid http method type: {httpMethod}")
        };
    }
}