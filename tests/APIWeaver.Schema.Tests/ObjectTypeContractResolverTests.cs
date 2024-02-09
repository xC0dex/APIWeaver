using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using APIWeaver.Schema.Repositories;
using Microsoft.OpenApi.Models;
using NSubstitute.ReturnsExtensions;

namespace APIWeaver.Schema.Tests;

public class ObjectTypeContractResolverTests
{
    private readonly ISchemaGenerator _schemaGenerator = Substitute.For<ISchemaGenerator>();
    private readonly ISchemaRepository _schemaRepository = Substitute.For<ISchemaRepository>();
    private readonly IValidationTransformer _validationTransformer = Substitute.For<IValidationTransformer>();

    private ObjectTypeContractResolver _sut;


    public ObjectTypeContractResolverTests()
    {
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, _schemaRepository);
    }

    [Fact]
    public void GenerateSchema_ShouldReturnReference_WhenSchemaReferenceExists()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var contract = new ObjectTypeContract(type, [], []);
        var existingSchema = new OpenApiSchema();
        _schemaRepository.GetSchemaReference(contract.Type).Returns(existingSchema);

        // Act
        var result = _sut.GenerateSchema(contract);

        // Assert
        result.Should().Be(existingSchema);
        _schemaRepository.Received().GetSchemaReference(type);
        _schemaGenerator.DidNotReceiveWithAnyArgs().GenerateSchema(default!, []);
    }

    [Fact]
    public void GenerateSchema_ShouldGenerateSchema_WhenSchemaReferenceNotExists()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var contract = new ObjectTypeContract(type, [], []);
        var repository = new SchemaRepository();
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, repository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        _validationTransformer.ReceivedWithAnyArgs().AddValidationRequirements(default!, []);
        var schemas = repository.GetSchemas();
        schemas.Should().HaveCount(1);
        var generatedSchema = schemas[type.Name];
        generatedSchema.Type.Should().Be(OpenApiDataType.Object.ToStringFast());
        generatedSchema.Properties.Should().NotBeNull();
        generatedSchema.AdditionalPropertiesAllowed.Should().BeFalse();
        generatedSchema.Required.Should().NotBeNull();
    }

    [Fact]
    public void GenerateSchemaForProperties_ShouldAddRequiredProperty_WhenPropertyHasRequiredAttribute()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var contract = new ObjectTypeContract(type, [new PropertyContract(typeof(string), "Name", false, [new RequiredAttribute()])], []);
        var repository = new SchemaRepository();
        _schemaGenerator.GenerateSchema(default!, []).ReturnsForAnyArgs(new OpenApiSchema());
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, repository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        _schemaGenerator.ReceivedWithAnyArgs().GenerateSchema(default!, []);
        var schema = repository.GetSchemas().First().Value;
        schema.Required.Should().HaveCount(1);
        schema.Properties.Should().HaveCount(1);
        var property = schema.Properties.First();
        property.Key.Should().Be("Name");
        property.Value.MinLength.Should().Be(1);
        property.Value.Nullable.Should().BeFalse();
    }

    [Fact]
    public void GenerateSchemaForProperties_ShouldAddRequiredPropertyWithNullable_WhenRequiredAttributeWithEmptyStringsAllowed()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var requiredAttribute = new RequiredAttribute
        {
            AllowEmptyStrings = true
        };
        var contract = new ObjectTypeContract(type, [new PropertyContract(typeof(string), "Name", false, [requiredAttribute])], []);
        var repository = new SchemaRepository();
        _schemaGenerator.GenerateSchema(default!, []).ReturnsForAnyArgs(new OpenApiSchema());
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, repository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        var schema = repository.GetSchemas().First().Value;
        var property = schema.Properties.First();
        property.Value.MinLength.Should().BeNull();
        property.Value.Nullable.Should().BeFalse();
    }

    [Fact]
    public void GenerateSchemaForProperties_ShouldSetNullableTrue_WhenEmptyStringsAndPropertyIsNullable()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var requiredAttribute = new RequiredAttribute
        {
            AllowEmptyStrings = true
        };
        var contract = new ObjectTypeContract(type, [new PropertyContract(typeof(string), "Name", true, [requiredAttribute])], []);
        var repository = new SchemaRepository();
        _schemaGenerator.GenerateSchema(default!, []).ReturnsForAnyArgs(new OpenApiSchema());
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, repository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        var schema = repository.GetSchemas().First().Value;
        var property = schema.Properties.First();
        property.Value.MinLength.Should().BeNull();
        property.Value.Nullable.Should().BeTrue();
    }

    [Fact]
    public void GenerateSchemaForProperties_ShouldAddRequired_WhenPropertyHasRequiredMemberAttribute()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var contract = new ObjectTypeContract(type, [new PropertyContract(typeof(string), "Name", false, [new RequiredMemberAttribute()])], []);
        var repository = new SchemaRepository();
        _schemaGenerator.GenerateSchema(default!, []).ReturnsForAnyArgs(new OpenApiSchema());
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, repository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        var schema = repository.GetSchemas().First().Value;
        schema.Required.Should().HaveCount(1);
    }

    [Fact]
    public void GenerateSchemaForProperties_ShouldAddDeprecated_WhenPropertyIsObsolete()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var contract = new ObjectTypeContract(type, [new PropertyContract(typeof(string), "Name", false, [new ObsoleteAttribute()])], []);
        var repository = new SchemaRepository();
        _schemaGenerator.GenerateSchema(default!, []).ReturnsForAnyArgs(new OpenApiSchema());
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, repository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        var schema = repository.GetSchemas().First().Value;
        var (_, propertySchema) = schema.Properties.First();
        propertySchema.Deprecated.Should().BeTrue();
    }

    [Fact]
    public void GenerateSchemaForProperties_ShouldRequestOneOfSchema_WhenReferenceIsNotNull()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var propertyType = typeof(string);
        var contract = new ObjectTypeContract(type, [new PropertyContract(propertyType, "Name", false, [new ObsoleteAttribute()])], []);
        _schemaRepository.GetSchemaReference(type).ReturnsNull();
        _schemaRepository.GetOneOfSchemaReference(propertyType).Returns(new OpenApiSchema());
        _schemaGenerator.GenerateSchema(default!, []).ReturnsForAnyArgs(new OpenApiSchema
        {
            Reference = new OpenApiReference()
        });
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, _schemaRepository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        _schemaRepository.Received().GetOneOfSchemaReference(propertyType);
    }

    [Fact]
    public void GenerateSchemaForProperties_ShouldRequestOneOfSchemaForUnderlyingType_WhenPropertyTypeIsNullable()
    {
        // Arrange
        var type = typeof(ObjectTypeContractResolverTests);
        var propertyType = typeof(int?);
        var contract = new ObjectTypeContract(type, [new PropertyContract(propertyType, "Name", false, [new ObsoleteAttribute()])], []);
        _schemaRepository.GetSchemaReference(type).ReturnsNull();
        _schemaRepository.GetOneOfSchemaReference(default!).ReturnsForAnyArgs(new OpenApiSchema());
        _schemaGenerator.GenerateSchema(default!, []).ReturnsForAnyArgs(new OpenApiSchema
        {
            Reference = new OpenApiReference()
        });
        _sut = new ObjectTypeContractResolver(_schemaGenerator, _validationTransformer, _schemaRepository);

        // Act
        _sut.GenerateSchema(contract);

        // Assert
        _schemaRepository.Received().GetOneOfSchemaReference(typeof(int));
    }
}