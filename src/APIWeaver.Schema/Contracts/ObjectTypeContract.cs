namespace APIWeaver.Schema.Contracts;

internal sealed record ObjectTypeContract(Type Type, IEnumerable<PropertyContract> Properties, IEnumerable<Attribute> CustomAttributes) : IContract;