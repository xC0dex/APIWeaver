namespace APIWeaver.Schema.Extensions;

internal static class OpenApiSchemaExtensions
{
    public static bool IsType(this OpenApiSchema schema, OpenApiDataType dataType)
    {
        return schema.Type == dataType.ToStringFast();
    }
}