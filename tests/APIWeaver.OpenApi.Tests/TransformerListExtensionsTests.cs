using APIWeaver.Core.Transformer;

namespace APIWeaver.OpenApi.Tests;

public class TransformerListExtensionsTests
{
    [Fact]
    public void Add_ShouldAddFunctionTransformer_WhenFunctionProvided()
    {
        // Arrange
        List<ITransformer<TransformContext>> list = [];

        // Act
        list.Add(_ => Task.CompletedTask);

        // Assert
        list.Should().HaveCount(1);
    }

    [Fact]
    public async Task Add_ShouldAddActionTransformer_WhenActionProvided()
    {
        // Arrange
        var invoked = false;
        List<ITransformer<TransformContext>> list = [];

        // Act
        list.Add(_ => { invoked = true; });
        await list[0].TransformAsync(null!);

        // Assert
        invoked.Should().BeTrue();
    }
}