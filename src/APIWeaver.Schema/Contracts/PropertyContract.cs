namespace APIWeaver.Schema.Contracts;

internal sealed record PropertyContract(Type Type, string Name, bool Nullable, IEnumerable<Attribute> CustomAttributes) : IContract;