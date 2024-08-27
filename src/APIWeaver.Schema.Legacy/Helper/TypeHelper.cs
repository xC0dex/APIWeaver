using System.Collections.Immutable;

namespace APIWeaver.Schema.Helper;

internal static class TypeHelper
{
    public static readonly ImmutableDictionary<Type, PrimitiveTypeDefinition> PrimitiveTypes = new Dictionary<Type, PrimitiveTypeDefinition>
    {
        {
            typeof(Guid),
            new PrimitiveTypeDefinition(OpenApiDataType.String, "uuid")
        },
        {
            typeof(byte),
            new PrimitiveTypeDefinition(OpenApiDataType.Integer, "int32")
        },
        {
            typeof(sbyte),
            new PrimitiveTypeDefinition(OpenApiDataType.Integer, "int32")
        },
        {
            typeof(byte[]),
            new PrimitiveTypeDefinition(OpenApiDataType.String, "byte")
        },
        {
            typeof(short),
            new PrimitiveTypeDefinition(OpenApiDataType.Integer, "int32")
        },
        {
            typeof(ushort),
            new PrimitiveTypeDefinition(OpenApiDataType.Integer, "int32")
        },
        {
            typeof(int),
            new PrimitiveTypeDefinition(OpenApiDataType.Integer, "int32")
        },
        {
            typeof(uint),
            new PrimitiveTypeDefinition(OpenApiDataType.Integer, "int32")
        },
        {
            typeof(long),
            new PrimitiveTypeDefinition(OpenApiDataType.Integer, "int64")
        },
        {
            typeof(ulong),
            new PrimitiveTypeDefinition(OpenApiDataType.Integer, "int64")
        },
        {
            typeof(float),
            new PrimitiveTypeDefinition(OpenApiDataType.Number, "float")
        },
        {
            typeof(double),
            new PrimitiveTypeDefinition(OpenApiDataType.Number, "double")
        },
        {
            typeof(decimal),
            new PrimitiveTypeDefinition(OpenApiDataType.Number, "double")
        },
        {
            typeof(bool),
            new PrimitiveTypeDefinition(OpenApiDataType.Boolean, null)
        },
        {
            typeof(string),
            new PrimitiveTypeDefinition(OpenApiDataType.String, null)
        },
        {
            typeof(char),
            new PrimitiveTypeDefinition(OpenApiDataType.String, null)
        },
        {
            typeof(Uri),
            new PrimitiveTypeDefinition(OpenApiDataType.String, "uri")
        },
        {
            typeof(DateTime),
            new PrimitiveTypeDefinition(OpenApiDataType.String, "date-time")
        },
        {
            typeof(DateTimeOffset),
            new PrimitiveTypeDefinition(OpenApiDataType.String, "date-time")
        },
        {
            typeof(DateOnly),
            new PrimitiveTypeDefinition(OpenApiDataType.String, "date")
        },
        {
            typeof(TimeSpan),
            new PrimitiveTypeDefinition(OpenApiDataType.String, "time")
        }
    }.ToImmutableDictionary();
}