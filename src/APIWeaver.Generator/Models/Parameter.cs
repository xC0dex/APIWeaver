namespace APIWeaver;

internal sealed class Parameter
{
    public required string Type { get; init; }

    public required bool Nullable { get; init; }

    public required string Name { get; init; }

    public string? DefaultValue { get; init; }
}