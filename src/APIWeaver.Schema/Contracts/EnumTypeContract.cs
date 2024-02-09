namespace APIWeaver.Schema.Contracts;

internal sealed record EnumTypeContract(PrimitiveTypeContract PrimitiveTypeContract, Type Type) : IContract;