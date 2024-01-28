using APIWeaver.Schema.Exceptions;
using Microsoft.OpenApi.Models;
using NSubstitute.ReturnsExtensions;

namespace APIWeaver.Schema.Tests;

public class SchemaGeneratorTests
{
    private readonly IContractFactory _contractFactory = Substitute.For<IContractFactory>();
    private readonly IContractResolverFactory _contractResolverFactory = Substitute.For<IContractResolverFactory>();
    private readonly IOptions<OpenApiSchemaGeneratorOptions> _schemaGeneratorOptions = Substitute.For<IOptions<OpenApiSchemaGeneratorOptions>>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();

    private readonly SchemaGenerator _sut;

    public SchemaGeneratorTests()
    {
        _sut = new SchemaGenerator(_contractFactory, _contractResolverFactory, _serviceProvider, _schemaGeneratorOptions);
    }

    [Fact]
    public void GenerateSchema_ShouldCallCorrectContractResolver_WhenTypeIsPrimitive()
    {
        // Arrange
        var type = typeof(string);
        Attribute[] attributes = [];
        var resolver = Substitute.For<IContractResolver<PrimitiveTypeContract>>();
        var contract = new PrimitiveTypeContract(new PrimitiveTypeDefinition(OpenApiDataType.String, null), attributes);
        _contractFactory.GetContract(type, attributes).Returns(contract);
        _contractResolverFactory.GetContractResolver<PrimitiveTypeContract>().Returns(resolver);

        // Act
        _sut.GenerateSchema(type, attributes);

        // Assert
        _contractFactory.Received().GetContract(type, attributes);
        _contractResolverFactory.Received().GetContractResolver<PrimitiveTypeContract>();
        resolver.Received().GenerateSchema(contract);
    }

    [Fact]
    public void GenerateSchema_ShouldCallCorrectContractResolver_WhenTypeIsEnum()
    {
        // Arrange
        var type = typeof(Enum);
        Attribute[] attributes = [];
        var resolver = Substitute.For<IContractResolver<EnumTypeContract>>();
        var contract = new EnumTypeContract(null!, type);
        _contractFactory.GetContract(type, attributes).Returns(contract);
        _contractResolverFactory.GetContractResolver<EnumTypeContract>().Returns(resolver);

        // Act
        _sut.GenerateSchema(type, attributes);

        // Assert
        _contractFactory.Received().GetContract(type, attributes);
        _contractResolverFactory.Received().GetContractResolver<EnumTypeContract>();
        resolver.Received().GenerateSchema(contract);
    }

    [Fact]
    public void GenerateSchema_ShouldCallCorrectContractResolver_WhenTypeIsArray()
    {
        // Arrange
        var type = typeof(string[]);
        Attribute[] attributes = [];
        var resolver = Substitute.For<IContractResolver<ArrayTypeContract>>();
        var contract = new ArrayTypeContract(type, false);
        _contractFactory.GetContract(type, attributes).Returns(contract);
        _contractResolverFactory.GetContractResolver<ArrayTypeContract>().Returns(resolver);

        // Act
        _sut.GenerateSchema(type, attributes);

        // Assert
        _contractFactory.Received().GetContract(type, attributes);
        _contractResolverFactory.Received().GetContractResolver<ArrayTypeContract>();
        resolver.Received().GenerateSchema(contract);
    }

    [Fact]
    public void GenerateSchema_ShouldCallCorrectContractResolver_WhenTypeIsDictionary()
    {
        // Arrange
        var type = typeof(Dictionary<,>);
        Attribute[] attributes = [];
        var resolver = Substitute.For<IContractResolver<DictionaryTypeContract>>();
        var contract = new DictionaryTypeContract(type);
        _contractFactory.GetContract(type, attributes).Returns(contract);
        _contractResolverFactory.GetContractResolver<DictionaryTypeContract>().Returns(resolver);

        // Act
        _sut.GenerateSchema(type, attributes);

        // Assert
        _contractFactory.Received().GetContract(type, attributes);
        _contractResolverFactory.Received().GetContractResolver<DictionaryTypeContract>();
        resolver.Received().GenerateSchema(contract);
    }

    [Fact]
    public void GenerateSchema_ShouldCallCorrectContractResolver_WhenTypeIsObject()
    {
        // Arrange
        var type = typeof(SchemaGeneratorTests);
        Attribute[] attributes = [];
        var resolver = Substitute.For<IContractResolver<ObjectTypeContract>>();
        var contract = new ObjectTypeContract(type, [], attributes);
        _contractFactory.GetContract(type, attributes).Returns(contract);
        _contractResolverFactory.GetContractResolver<ObjectTypeContract>().Returns(resolver);

        // Act
        _sut.GenerateSchema(type, attributes);

        // Assert
        _contractFactory.Received().GetContract(type, attributes);
        _contractResolverFactory.Received().GetContractResolver<ObjectTypeContract>();
        resolver.Received().GenerateSchema(contract);
    }

    [Fact]
    public void GenerateSchema_ShouldCallCorrectContractResolver_WhenTypeUndefined()
    {
        // Arrange
        var type = typeof(object);
        Attribute[] attributes = [];
        var resolver = Substitute.For<IContractResolver<UndefinedTypeContract>>();
        var contract = new UndefinedTypeContract();
        _contractFactory.GetContract(type, attributes).Returns(contract);
        _contractResolverFactory.GetContractResolver<UndefinedTypeContract>().Returns(resolver);

        // Act
        _sut.GenerateSchema(type, attributes);

        // Assert
        _contractFactory.Received().GetContract(type, attributes);
        _contractResolverFactory.Received().GetContractResolver<UndefinedTypeContract>();
        resolver.Received().GenerateSchema(contract);
    }

    [Fact]
    public void GenerateSchema_ShouldCallGetContractWithUnderlyingType_WhenTypeIsNullable()
    {
        // Arrange
        var type = typeof(int?);
        var expectedType = typeof(int);
        Attribute[] attributes = [];
        var resolver = Substitute.For<IContractResolver<PrimitiveTypeContract>>();
        var contract = new PrimitiveTypeContract(new PrimitiveTypeDefinition(OpenApiDataType.Integer, null), attributes);
        _contractFactory.GetContract(expectedType, attributes).Returns(contract);
        _contractResolverFactory.GetContractResolver<PrimitiveTypeContract>().Returns(resolver);

        // Act
        _sut.GenerateSchema(type, attributes);

        // Assert
        _contractFactory.Received().GetContract(expectedType, attributes);
    }

    [Fact]
    public void GenerateSchema_ShouldThrowUnknownContractException_WhenContractIsUnknown()
    {
        // Arrange
        _contractFactory.GetContract(default!, default!).ReturnsNullForAnyArgs();

        // Act
        var act = () => _sut.GenerateSchema(typeof(object), []);

        // Assert
        act.Should().Throw<UnknownContractException>();
    }

    [Fact]
    public async Task GenerateSchemaAsync_ShouldCallSchemaTransformers_WhenOptionsNotEmpty()
    {
        // Arrange
        var type = typeof(int);
        Attribute[] attributes = [];

        // For GetSchema 
        var schema = new OpenApiSchema();
        var resolver = Substitute.For<IContractResolver<PrimitiveTypeContract>>();
        resolver.GenerateSchema(default!).ReturnsForAnyArgs(schema);
        var contract = new PrimitiveTypeContract(new PrimitiveTypeDefinition(OpenApiDataType.Integer, null), attributes);
        _contractFactory.GetContract(type, attributes).Returns(contract);
        _contractResolverFactory.GetContractResolver<PrimitiveTypeContract>().Returns(resolver);

        // For GetSchemaAsync
        var options = new OpenApiSchemaGeneratorOptions();
        var transformer = Substitute.For<ISchemaTransformer>();
        transformer.TransformAsync(default!).ReturnsForAnyArgs(Task.Delay(69));
        options.SchemaTransformers.Add(transformer);
        _schemaGeneratorOptions.Value.Returns(options);

        // Act
        await _sut.GenerateSchemaAsync(type, attributes, CancellationToken.None);

        // Assert
        await transformer.Received().TransformAsync(Arg.Is<SchemaContext>(s => s.Schema == schema));
    }
}