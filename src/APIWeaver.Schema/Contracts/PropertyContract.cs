namespace APIWeaver.Schema.Contracts;

internal sealed record PropertyContract(Type Type, string Name, bool Nullable, bool Readonly, bool WriteOnly, IEnumerable<Attribute> CustomAttributes) : IContract;