using System.ComponentModel;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Schema.Tests;

public class DictionaryTypeContractResolverTests
{
    private readonly ISchemaGenerator _schemaGenerator = Substitute.For<ISchemaGenerator>();
    private readonly DictionaryTypeContractResolver _sut;

    public DictionaryTypeContractResolverTests()
    {
        _sut = new DictionaryTypeContractResolver(_schemaGenerator);
    }

    [Fact]
    public void GenerateSchema_ShouldReturnCorrectSchema_WhenItemTypeIsString()
    {
        // Arrange
        var itemType = typeof(string);
        var contract = new DictionaryTypeContract(itemType);
        var itemSchema = new OpenApiSchema();

        _schemaGenerator.GenerateSchema(itemType, Arg.Any<IEnumerable<Attribute>>()).Returns(itemSchema);

        // Act
        var schema = _sut.GenerateSchema(contract);

        // Assert
        _schemaGenerator.Received().GenerateSchema(itemType, Arg.Any<IEnumerable<Attribute>>());
        schema.Type.Should().Be(OpenApiDataType.Object.ToStringFast());
        schema.AdditionalPropertiesAllowed.Should().BeTrue();
        schema.AdditionalProperties.Should().Be(itemSchema);
    }

    [Fact]
    public void GenerateSchema_ShouldCallGeneratorWithCorrectAttributes_WhenTypeHasAttributes()
    {
        // Arrange
        var itemType = typeof(ItemType);
        var contract = new DictionaryTypeContract(itemType);
        var itemSchema = new OpenApiSchema();

        _schemaGenerator.GenerateSchema(itemType, Arg.Any<IEnumerable<Attribute>>()).Returns(itemSchema);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        _schemaGenerator.Received().GenerateSchema(itemType, Arg.Is<IEnumerable<Attribute>>(i => i.OfType<DescriptionAttribute>().Any()));
    }
}

[Description]
file class ItemType;