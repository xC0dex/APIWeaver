namespace APIWeaver;

internal sealed record HeaderValue
{
    public required string Name { get; init; }

    public required string Type { get; init; }

    public required string Default { get; init; }

    public required bool Required { get; init; }
}