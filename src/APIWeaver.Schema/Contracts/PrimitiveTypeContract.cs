namespace APIWeaver.Schema.Contracts;

internal sealed record PrimitiveTypeContract(PrimitiveTypeDefinition PrimitiveTypeDefinition, IEnumerable<Attribute> CustomAttributes) : IContract;