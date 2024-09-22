using System.Text.Json.Serialization;

namespace APIWeaver;

internal sealed class GeneratorConfiguration
{
    [JsonIgnore]
    internal string ExecutionContext { get; set; } = null!;

    [JsonIgnore]
    internal string FullDocumentPath => Path.Combine(ExecutionContext, DocumentPath);

    [JsonIgnore]
    internal string FullOutputPath => Path.Combine(ExecutionContext, OutputPath);

    public required string DocumentPath { get; init; }

    public required string OutputPath { get; init; }

    public required string Namespace { get; init; }

    public string NamePattern { get; init; } = "{tag}Client";
}

[JsonSerializable(typeof(GeneratorConfiguration))]
[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class ConfigurationSerializerContext : JsonSerializerContext;