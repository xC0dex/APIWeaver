using APIWeaver.Schema.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver.Schema.Tests;

public class ContractResolverFactoryTests
{
    [Fact]
    public void GetContractResolver_ShouldReturnResolver_WhenFoundInServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<IContractResolver<UndefinedTypeContract>, UndefinedTypeContractResolver>();
        var provider = services.BuildServiceProvider();
        var sut = new ContractResolverFactory(provider);

        // Act
        var resolver = sut.GetContractResolver<UndefinedTypeContract>();

        // Assert
        resolver.Should().BeOfType<UndefinedTypeContractResolver>();
    }

    [Fact]
    public void GetContractResolver_ShouldThrowContractResolverNotFoundException_WhenNotFoundInServiceCollection()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var sut = new ContractResolverFactory(provider);

        // Act
        var act = () => sut.GetContractResolver<UndefinedTypeContract>();

        // Assert
        act.Should().Throw<ContractResolverNotFoundException>().WithMessage("No contract found for type `UndefinedTypeContract`");
    }
}