namespace APIWeaver.Schema.Factories;

internal interface IContractResolverFactory
{
    IContractResolver<TContract> GetContractResolver<TContract>() where TContract : IContract;
}