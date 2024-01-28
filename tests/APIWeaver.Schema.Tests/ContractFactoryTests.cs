using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.Schema.Tests;

public class ContractFactoryTests
{
    private readonly IOptions<JsonOptions> _controllerJsonOptions = Substitute.For<IOptions<JsonOptions>>();
    private readonly IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions> _minimalApiJsonOptions = Substitute.For<IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>>();
    private readonly IOptions<OpenApiSchemaGeneratorOptions> _schemaGeneratorOptions = Substitute.For<IOptions<OpenApiSchemaGeneratorOptions>>();

    private readonly ContractFactory _sut;

    public ContractFactoryTests()
    {
        _controllerJsonOptions.Value.Returns(new JsonOptions());
        _minimalApiJsonOptions.Value.Returns(new Microsoft.AspNetCore.Http.Json.JsonOptions());
        _schemaGeneratorOptions.Value.Returns(new OpenApiSchemaGeneratorOptions());
        _sut = new ContractFactory(_schemaGeneratorOptions, _controllerJsonOptions, _minimalApiJsonOptions);
    }

    [Fact]
    public void GetContract_ShouldReturnCorrectContract_WhenTypeIsPrimitive()
    {
        // Arrange
        var type = typeof(int);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<PrimitiveTypeContract>()
            .Which.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.Integer);
    }

    [Fact]
    public void GetContract_ShouldReturnCorrectContract_WhenTypeIsUndefined()
    {
        // Arrange
        var type = typeof(JsonElement);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<UndefinedTypeContract>();
    }

    [Fact]
    public void GetContract_ShouldReturnEnumTypeContract_WhenTypeIsEnum()
    {
        // Arrange
        var type = typeof(DayOfWeek);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<EnumTypeContract>()
            .Which.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.Integer);
    }

    [Fact]
    public void GetContract_ShouldReturnEnumTypeContractWithString_WhenAttributesHaveJsonConverterAttribute()
    {
        // Arrange
        var type = typeof(DayOfWeek);
        Attribute[] attributes = [new JsonConverterAttribute(typeof(JsonStringEnumConverter))];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<EnumTypeContract>()
            .Which.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.String);
    }

    [Fact]
    public void GetContract_ShouldReturnEnumTypeContractWithString_WhenEnumTypeHasJsonStringConverterAttribute()
    {
        // Arrange
        var type = typeof(TestEnum);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<EnumTypeContract>()
            .Which.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.String);
    }

    [Fact]
    public void GetContract_ShouldReturnEnumTypeContractWithString_WhenMinimalApiOptionsHaveJsonStringConverter()
    {
        // Arrange
        var type = typeof(DayOfWeek);
        Attribute[] attributes = [];
        var options = new Microsoft.AspNetCore.Http.Json.JsonOptions();
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        _minimalApiJsonOptions.Value.Returns(options);
        var sut = new ContractFactory(_schemaGeneratorOptions, _controllerJsonOptions, _minimalApiJsonOptions);

        // Act
        var contract = sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<EnumTypeContract>()
            .Which.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.String);
    }

    [Fact]
    public void GetContract_ShouldReturnEnumTypeContractWithString_WhenControllerOptionsHaveJsonStringConverter()
    {
        // Arrange
        var type = typeof(DayOfWeek);
        Attribute[] attributes = [];
        var options = new JsonOptions();
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        _controllerJsonOptions.Value.Returns(options);
        _schemaGeneratorOptions.Value.Returns(new OpenApiSchemaGeneratorOptions
        {
            JsonOptionsSource = JsonOptionsSource.ControllerOptions
        });
        var sut = new ContractFactory(_schemaGeneratorOptions, _controllerJsonOptions, _minimalApiJsonOptions);

        // Act
        var contract = sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<EnumTypeContract>()
            .Which.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.String);
    }

    [Fact]
    public void GetContract_ShouldReturnDictionaryTypeContract_WhenTypeIsDictionary()
    {
        // Arrange
        var type = typeof(Dictionary<string, int>);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<DictionaryTypeContract>()
            .Which.ValueType.Should().Be(typeof(int));
    }


    [Fact]
    public void GetContract_ShouldReturnDictionaryWithObjectValueType_WhenDictionaryIsNotGeneric()
    {
        // Arrange
        var type = typeof(IDictionary);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<DictionaryTypeContract>()
            .Which.ValueType.Should().Be(typeof(object));
    }

    [Fact]
    public void GetContract_ShouldReturnArrayTypeContract_WhenTypeIsArray()
    {
        // Arrange
        var type = typeof(string[]);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<ArrayTypeContract>()
            .Which.ItemType.Should().Be(typeof(string));
    }

    [Fact]
    public void GetContract_ShouldReturnArrayTypeContract_WhenTypeIsGenericIEnumerable()
    {
        // Arrange
        var type = typeof(IEnumerable<int>);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<ArrayTypeContract>()
            .Which.ItemType.Should().Be(typeof(int));
    }

    [Fact]
    public void GetContract_ShouldReturnArrayTypeContract_WhenTypeIsIEnumerable()
    {
        // Arrange
        var type = typeof(IEnumerable);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<ArrayTypeContract>()
            .Which.ItemType.Should().Be(typeof(object));
    }

    [Fact]
    public void GetContract_ShouldReturnObjectTypeContract_WhenTypeIsClass()
    {
        // Arrange
        var type = typeof(User);
        Attribute[] attributes = [];

        // Act
        var contract = _sut.GetContract(type, attributes);

        // Assert
        contract.Should().BeOfType<ObjectTypeContract>()
            .Which.Type.Should().Be(type);

        var properties = (contract as ObjectTypeContract)!.Properties.ToArray();
        properties.Should().HaveCount(4);

        // Asserting on the properties
        var nameProperty = properties.First(x => x.Name == "name");
        nameProperty.Type.Should().Be(typeof(string));
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
file enum TestEnum;

#pragma warning disable CS0169 // Field is never used
file class User
{
    private string? _company;

    [JsonInclude]
    [JsonPropertyName("dev")]
    private bool _isDeveloper;

    public required string Name { get; set; }

    [JsonIgnore]
    public bool Enabled { get; set; }

    [JsonInclude]
    private int Age { get; set; }

    public string? Bio { get; set; }
}
#pragma warning restore CS0169 // Field is never used