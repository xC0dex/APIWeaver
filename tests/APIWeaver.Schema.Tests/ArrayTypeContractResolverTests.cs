using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Schema.Tests;

public class ArrayTypeContractResolverTests
{
    private readonly ISchemaGenerator _schemaGenerator = Substitute.For<ISchemaGenerator>();
    private readonly ArrayTypeContractResolver _sut;
    private readonly IValidationTransformer _validationTransformer = Substitute.For<IValidationTransformer>();

    public ArrayTypeContractResolverTests()
    {
        _sut = new ArrayTypeContractResolver(_schemaGenerator, _validationTransformer);
    }

    [Fact]
    public void GenerateSchema_ShouldReturnCorrectSchema_WhenContractHasUniqueItems()
    {
        // Arrange
        var itemType = typeof(string);
        var contract = new ArrayTypeContract(itemType, true, []);
        var itemSchema = new OpenApiSchema();

        _schemaGenerator.GenerateSchema(itemType, Arg.Any<IEnumerable<Attribute>>()).Returns(itemSchema);

        // Act
        var schema = _sut.GenerateSchema(contract);

        // Assert
        _schemaGenerator.Received().GenerateSchema(itemType, Arg.Any<IEnumerable<Attribute>>());
        schema.Type.Should().Be(OpenApiDataType.Array.ToStringFast());
        schema.UniqueItems.Should().BeTrue();
        schema.Items.Should().Be(itemSchema);
    }

    [Fact]
    public void GenerateSchema_ShouldCallGeneratorWithCorrectAttributes_WhenTypeHasAttributes()
    {
        // Arrange
        var itemType = typeof(ItemType);
        var contract = new ArrayTypeContract(itemType, true, []);
        var itemSchema = new OpenApiSchema();

        _schemaGenerator.GenerateSchema(itemType, Arg.Any<IEnumerable<Attribute>>()).Returns(itemSchema);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        _schemaGenerator.Received().GenerateSchema(itemType, Arg.Is<IEnumerable<Attribute>>(i => i.OfType<DescriptionAttribute>().Any()));
    }
    
    [Fact]
    public void GenerateSchema_ShouldCallValidationTransformer_WithGivenValidationAttributes()
    {
        // Arrange
        var itemType = typeof(string);
        var contract = new ArrayTypeContract(itemType, true, [new MinLengthAttribute(69)]);
        var itemSchema = new OpenApiSchema();

        _schemaGenerator.GenerateSchema(itemType, Arg.Any<IEnumerable<Attribute>>()).Returns(itemSchema);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
       _validationTransformer.Received().AddValidationRequirements(Arg.Any<OpenApiSchema>(), Arg.Is<IEnumerable<ValidationAttribute>>(i => i.OfType<MinLengthAttribute>().Any()));
    }
}

[Description]
file class ItemType;