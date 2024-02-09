using APIWeaver.Schema.Repositories;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using NSubstitute.ReturnsExtensions;

namespace APIWeaver.Schema.Tests;

public class EnumTypeContractResolverTests
{
    private readonly ISchemaRepository _schemaRepository = Substitute.For<ISchemaRepository>();
    private EnumTypeContractResolver _sut;

    public EnumTypeContractResolverTests()
    {
        _sut = new EnumTypeContractResolver(_schemaRepository);
    }

    [Fact]
    public void GenerateSchema_ShouldGenerateSchema_WhenSchemaRepositoryReturnsNull()
    {
        // Arrange
        var enumType = typeof(DayOfWeek);
        var primitiveTypeContract = new PrimitiveTypeContract(TypeHelper.PrimitiveTypes[typeof(int)], []);
        var contract = new EnumTypeContract(primitiveTypeContract, enumType);
        var schemaReference = new OpenApiSchema();
        _schemaRepository.GetSchemaReference(enumType).ReturnsNull();
        _schemaRepository.AddOpenApiSchema(enumType, Arg.Any<OpenApiSchema>()).Returns(schemaReference);

        // Act
        var schema = _sut.GenerateSchema(contract);

        // Assert
        _schemaRepository.Received().GetSchemaReference(enumType);
        _schemaRepository.Received().AddOpenApiSchema(enumType, Arg.Any<OpenApiSchema>());
        schema.Should().Be(schemaReference);
    }

    [Fact]
    public void GenerateSchema_ShouldGenerateSchemaWithIntegerType_WhenTypeIsInt()
    {
        // Arrange
        var enumType = typeof(DayOfWeek);
        var primitiveTypeContract = new PrimitiveTypeContract(TypeHelper.PrimitiveTypes[typeof(int)], []);
        var contract = new EnumTypeContract(primitiveTypeContract, enumType);
        _schemaRepository.GetSchemaReference(enumType).ReturnsNull();

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        _schemaRepository.Received().GetSchemaReference(enumType);
        _schemaRepository.Received().AddOpenApiSchema(enumType, Arg.Is<OpenApiSchema>(o => o.Type == OpenApiDataType.Integer.ToStringFast()));
    }

    [Fact]
    public void GenerateSchema_ShouldGenerateSchemaWithStringType_WhenTypeIsString()
    {
        // Arrange
        var enumType = typeof(DayOfWeek);
        var primitiveTypeContract = new PrimitiveTypeContract(TypeHelper.PrimitiveTypes[typeof(string)], []);
        var contract = new EnumTypeContract(primitiveTypeContract, enumType);
        var repository = new SchemaRepository();
        _sut = new EnumTypeContractResolver(repository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        var schemas = repository.GetSchemas();
        schemas.Should().HaveCount(1);
        var generatedSchema = schemas[enumType.Name];
        const string expected = """
                                {
                                  "enum": [
                                    "Sunday",
                                    "Monday",
                                    "Tuesday",
                                    "Wednesday",
                                    "Thursday",
                                    "Friday",
                                    "Saturday"
                                  ],
                                  "type": "string"
                                }
                                """;
        var json = generatedSchema.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0);
        json.ReplaceLineEndings().Should().Be(expected);
    }

    [Fact]
    public void GenerateSchema_ShouldReturnReference_WhenAlreadyExists()
    {
        // Arrange
        var enumType = typeof(DayOfWeek);
        var primitiveTypeContract = new PrimitiveTypeContract(TypeHelper.PrimitiveTypes[typeof(string)], []);
        var expected = new OpenApiSchema();
        _schemaRepository.GetSchemaReference(enumType).Returns(expected);
        var contract = new EnumTypeContract(primitiveTypeContract, enumType);

        // Act
        var result = _sut.GenerateSchema(contract);

        // Assert
        result.Should().Be(expected);
    }
}