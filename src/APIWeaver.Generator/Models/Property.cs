namespace APIWeaver;

internal sealed class Property
{
    public required AccessModifier AccessModifier { get; init; }

    public required bool Required { get; init; }

    public required string Name { get; init; }

    public required string Type { get; init; }

    public string? ExpressionBody { get; init; }

    public required bool Nullable { get; init; }
}