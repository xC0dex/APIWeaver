using System.Text.Json.Serialization;

namespace APIWeaver;

internal sealed class GeneratorConfiguration
{
    public required string OpenApiDocumentPath { get; init; }

    public required string OutputPath { get; init; }
}

[JsonSerializable(typeof(GeneratorConfiguration))]
[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class ConfigurationSerializerContext: JsonSerializerContext;