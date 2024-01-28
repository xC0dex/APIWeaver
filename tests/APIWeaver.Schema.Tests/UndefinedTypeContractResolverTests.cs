namespace APIWeaver.Schema.Tests;

public class UndefinedTypeContractResolverTests
{
    [Fact]
    public void GenerateSchema_ShouldReturnEmptySchema_WhenCalled()
    {
        // Arrange
        var sut = new UndefinedTypeContractResolver();

        // Act
        var schema = sut.GenerateSchema(new UndefinedTypeContract());

        // Assert
        schema.Should().NotBeNull();
    }
}