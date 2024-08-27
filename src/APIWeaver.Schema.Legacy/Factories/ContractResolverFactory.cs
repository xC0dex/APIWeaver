using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver.Schema.Factories;

internal sealed class ContractResolverFactory(IServiceProvider serviceProvider) : IContractResolverFactory
{
    public IContractResolver<TContract> GetContractResolver<TContract>() where TContract : IContract
    {
        var resolver = serviceProvider.GetService<IContractResolver<TContract>>();
        return resolver ?? throw new ContractResolverNotFoundException(typeof(TContract));
    }
}