namespace APIWeaver;

internal sealed record Class
{
    public required AccessModifier AccessModifier { get; init; }

    public required ClassType Type { get; init; }

    public List<TypeParameter> TypeParameters { get; init; } = [];

    public List<Parameter> ConstructorParameters { get; init; } = [];

    public required string Name { get; init; }

    public List<Method>? Methods { get; init; }

    public List<Property>? Properties { get; init; }
}