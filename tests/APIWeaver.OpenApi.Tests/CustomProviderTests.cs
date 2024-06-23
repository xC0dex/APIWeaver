namespace APIWeaver.OpenApi.Tests;

public class CustomProviderTests
{
    [Fact]
    public void Test()
    {

        // Act
        var result = CustomProvider.Provide(true, "test");

        // Assert
        result.Should().Be("test");
    }
}