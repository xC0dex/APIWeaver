namespace APIWeaver;

/// <summary>
/// Represents a URL.
/// </summary>
public sealed record Url(string Name, [property: JsonPropertyName("url")] string Route);