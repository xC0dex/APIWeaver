namespace APIWeaver.Schema.Contracts;

internal sealed record ArrayTypeContract(Type ItemType, bool UniqueItems, IEnumerable<Attribute> CustomAttributes) : IContract;