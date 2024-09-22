namespace APIWeaver;

internal sealed record CSharpFile
{
    public required List<string> Usings { get; init; }

    public required string Namespace { get; init; }

    public required List<string> PreProcessorDirectives { get; init; }

    public required string Name { get; init; }

    public required List<Class> Classes { get; init; }
}