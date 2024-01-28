namespace APIWeaver.Schema.Contracts;

internal sealed record ArrayTypeContract(Type ItemType, bool UniqueItems) : IContract;