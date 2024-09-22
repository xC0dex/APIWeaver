namespace APIWeaver;

internal sealed record ResponseType
{
    public required string Name { get; init; }

    public required int StatusCode { get; init; }
}