using APIWeaver.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver.Tests;

public class ServiceCollectionExtensionsTest
{
    [Fact]
    public void AddApiWeaver_ShouldDoNothing_WhenCalled()
    {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        var result = services.AddApiWeaver();

        // Assert
        result.Should().BeSameAs(services);

    }
}