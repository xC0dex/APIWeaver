using System.Text.Json;

namespace APIWeaver.Swagger.Helper;

internal static class JsonSerializerHelper
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}