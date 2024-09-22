namespace APIWeaver;

internal sealed class Method
{
    public required AccessModifier AccessModifier { get; init; }
    
    public required ResponseType[] ResponseTypes { get; init; }

    public required string Name { get; init; }

    public required OperationType HttpMethod { get; init; }

    //TODO: Header values, parameters etc or maybe a different layer?
}