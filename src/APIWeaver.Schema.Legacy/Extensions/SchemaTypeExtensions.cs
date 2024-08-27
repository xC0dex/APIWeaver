namespace APIWeaver.Schema.Extensions;

internal static class SchemaTypeExtensions
{
    public static string ToStringFast(this OpenApiDataType type)
    {
        return type switch
        {
            OpenApiDataType.String => "string",
            OpenApiDataType.Number => "number",
            OpenApiDataType.Integer => "integer",
            OpenApiDataType.Boolean => "boolean",
            OpenApiDataType.Array => "array",
            OpenApiDataType.Object => "object",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}