namespace APIWeaver.Schema.Tests;

public class PrimitiveTypeContractResolverTests
{
    [Fact]
    public void GenerateSchema_ShouldReturnValidSchemaAndCallValidationTransformer_WhenCalled()
    {
        // Arrange
        var validationTransformer = Substitute.For<IValidationTransformer>();
        var contract = new PrimitiveTypeContract(TypeHelper.PrimitiveTypes[typeof(string)], []);
        var sut = new PrimitiveTypeContractResolver(validationTransformer);

        // Act
        var schema = sut.GenerateSchema(contract);

        // Assert
        schema.Type.Should().Be(contract.PrimitiveTypeDefinition.Type.ToStringFast());
        schema.Format.Should().Be(contract.PrimitiveTypeDefinition.Format);
        validationTransformer.ReceivedWithAnyArgs().AddValidationRequirements(default!, []);
    }
}