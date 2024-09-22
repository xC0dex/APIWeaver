namespace APIWeaver;

internal sealed record Method
{
    public required AccessModifier AccessModifier { get; init; }
    
    public required ResponseType[] ResponseTypes { get; init; }

    public required string Name { get; init; }

    public required OperationType HttpMethod { get; init; }

    public required Action<IndentedStringBuilder, Method> BodyFunc { get; init; }

    public required List<Parameter> Parameters { get; init; }

    //TODO: Header values, parameters etc or maybe a different layer?
}