namespace APIWeaver.Schema.Extensions;

internal static class OpenApiSchemaExtensions
{
    public static bool IsType(this OpenApiSchema schema, OpenApiDataType dataType) => schema.Type == dataType.ToStringFast();
}