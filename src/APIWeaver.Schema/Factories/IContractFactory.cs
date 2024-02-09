namespace APIWeaver.Schema.Factories;

internal interface IContractFactory
{
    IContract GetContract(Type type, IEnumerable<Attribute> customAttributes);
}