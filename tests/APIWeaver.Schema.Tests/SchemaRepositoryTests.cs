using APIWeaver.Schema.Repositories;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Schema.Tests;

public class SchemaRepositoryTests
{
    [Fact]
    public void GetSchemaReference_ShouldReturnSchemaReference_WhenTypeIsAlreadyInCache()
    {
        // Arrange
        var type = typeof(int);
        var sut = new SchemaRepository();
        sut.AddOpenApiSchema(type, new OpenApiSchema());

        // Act
        var result = sut.GetSchemaReference(type);

        // Assert
        result!.Reference.Should().NotBeNull();
    }

    [Fact]
    public void GetSchemaReference_ShouldReturnNull_WhenTypeIsNotInCache()
    {
        // Arrange
        var type = typeof(int);
        var sut = new SchemaRepository();

        // Act
        var result = sut.GetSchemaReference(type);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetOneOfSchemaReference_ShouldReturnSchemaWithOneOfSet()
    {
        // Arrange
        var type = typeof(int);
        var schemaRepository = new SchemaRepository();

        // Act
        var result = schemaRepository.GetOneOfSchemaReference(type);

        // Assert
        result.Should().NotBeNull();
        result.OneOf.Should().HaveCount(1);
    }


    [Fact]
    public void GetSchemas_ShouldReturnAllNonNullSchemasInCache()
    {
        // Arrange
        var type1 = typeof(int);
        var type2 = typeof(string);
        var schema1 = new OpenApiSchema();
        var schemaRepository = new SchemaRepository();
        schemaRepository.AddOpenApiSchema(type1, schema1);
        schemaRepository.AddOpenApiSchema(type2, null!);

        // Act
        var result = schemaRepository.GetSchemas();

        // Assert
        result.Should().ContainKey(type1.Name);
        result.Should().NotContainKey(type2.Name);
    }
}