using System.ComponentModel;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Schema.Tests;

public class ArrayTypeContractResolverTests
{
    private readonly ISchemaGenerator _schemaGenerator = Substitute.For<ISchemaGenerator>();
    private readonly ArrayTypeContractResolver _sut;

    public ArrayTypeContractResolverTests()
    {
        _sut = new ArrayTypeContractResolver(_schemaGenerator);
    }

    [Fact]
    public void GenerateSchema_ShouldReturnCorrectSchema_WhenContractHasUniqueItems()
    {
        // Arrange
        var itemType = typeof(string);
        var contract = new ArrayTypeContract(itemType, true);
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
        var contract = new ArrayTypeContract(itemType, true);
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