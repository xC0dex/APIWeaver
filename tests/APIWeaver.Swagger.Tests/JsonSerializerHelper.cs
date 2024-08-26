using System.Text.Json.Serialization;

namespace APIWeaver.Swagger.Tests;

public class JsonSerializerHelper
{
    public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}