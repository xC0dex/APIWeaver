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

    private ContractFactory _sut;

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

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<PrimitiveTypeContract>()
            .Which.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.Integer);
    }

    [Fact]
    public void GetContract_ShouldReturnCorrectContract_WhenTypeIsUndefined()
    {
        // Arrange
        var type = typeof(JsonElement);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<UndefinedTypeContract>();
    }

    [Fact]
    public void GetContract_ShouldReturnEnumTypeContract_WhenTypeIsEnum()
    {
        // Arrange
        var type = typeof(DayOfWeek);

        // Act
        var contract = _sut.GetContract(type, []);

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

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<EnumTypeContract>()
            .Which.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.String);
    }

    [Fact]
    public void GetContract_ShouldReturnEnumTypeContractWithString_WhenMinimalApiOptionsHaveJsonStringConverter()
    {
        // Arrange
        var type = typeof(DayOfWeek);
        var options = new Microsoft.AspNetCore.Http.Json.JsonOptions();
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        _minimalApiJsonOptions.Value.Returns(options);
        var sut = new ContractFactory(_schemaGeneratorOptions, _controllerJsonOptions, _minimalApiJsonOptions);

        // Act
        var contract = sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<EnumTypeContract>()
            .Which.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.String);
    }

    [Fact]
    public void GetContract_ShouldReturnEnumTypeContractWithString_WhenControllerOptionsHaveJsonStringConverter()
    {
        // Arrange
        var type = typeof(DayOfWeek);
        var options = new JsonOptions();
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        _controllerJsonOptions.Value.Returns(options);
        _schemaGeneratorOptions.Value.Returns(new OpenApiSchemaGeneratorOptions
        {
            JsonOptionsSource = JsonOptionsSource.ControllerOptions
        });
        var sut = new ContractFactory(_schemaGeneratorOptions, _controllerJsonOptions, _minimalApiJsonOptions);

        // Act
        var contract = sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<EnumTypeContract>()
            .Which.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.Should().Be(OpenApiDataType.String);
    }

    [Fact]
    public void GetContract_ShouldReturnDictionaryTypeContract_WhenTypeIsDictionary()
    {
        // Arrange
        var type = typeof(Dictionary<string, int>);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<DictionaryTypeContract>()
            .Which.ValueType.Should().Be(typeof(int));
    }


    [Fact]
    public void GetContract_ShouldReturnDictionaryWithObjectValueType_WhenDictionaryIsNotGeneric()
    {
        // Arrange
        var type = typeof(IDictionary);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<DictionaryTypeContract>()
            .Which.ValueType.Should().Be(typeof(object));
    }

    [Fact]
    public void GetContract_ShouldReturnArrayTypeContract_WhenTypeIsArray()
    {
        // Arrange
        var type = typeof(string[]);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ArrayTypeContract>()
            .Which.ItemType.Should().Be(typeof(string));
    }

    [Fact]
    public void GetContract_ShouldReturnArrayTypeContract_WhenTypeIsGenericIEnumerable()
    {
        // Arrange
        var type = typeof(IEnumerable<int>);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ArrayTypeContract>()
            .Which.ItemType.Should().Be(typeof(int));
    }

    [Fact]
    public void GetContract_ShouldReturnArrayTypeContract_WhenTypeIsIEnumerable()
    {
        // Arrange
        var type = typeof(IEnumerable);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ArrayTypeContract>()
            .Which.ItemType.Should().Be(typeof(object));
    }

    [Fact]
    public void GetContract_ShouldReturnObjectTypeContract_WhenTypeIsClass()
    {
        // Arrange
        var type = typeof(UserForFields);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ObjectTypeContract>()
            .Which.Type.Should().Be(type);

        var properties = (contract as ObjectTypeContract)!.Properties.ToArray();
        properties.Should().HaveCount(1);

        // Asserting on the properties
        properties.Should().Contain(x => x.Name == "dev");
    }

    [Fact]
    public void GetContract_ShouldIncludeFields_WhenOptionsSet()
    {
        // Arrange
        var type = typeof(UserForFields);
        var options = new Microsoft.AspNetCore.Http.Json.JsonOptions
        {
            SerializerOptions =
            {
                IncludeFields = true
            }
        };
        _minimalApiJsonOptions.Value.Returns(options);
        _sut = new ContractFactory(_schemaGeneratorOptions, _controllerJsonOptions, _minimalApiJsonOptions);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ObjectTypeContract>()
            .Which.Type.Should().Be(type);

        var properties = (contract as ObjectTypeContract)!.Properties.ToArray();
        properties.Should().HaveCount(3);

        // Asserting on the properties

        properties.Should().Contain(x => x.Name == "password");
        properties.Should().Contain(x => x.Name == "dev");
        properties.Should().Contain(x => x.Name == "fullName" && x.Readonly);
    }

    [Fact]
    public void GetContract_ShouldReturnIgnoreReadOnlyFields_WhenOptionsSet()
    {
        // Arrange
        var type = typeof(UserForFields);
        var options = new Microsoft.AspNetCore.Http.Json.JsonOptions
        {
            SerializerOptions =
            {
                IncludeFields = true,
                IgnoreReadOnlyFields = true
            }
        };
        _minimalApiJsonOptions.Value.Returns(options);
        _sut = new ContractFactory(_schemaGeneratorOptions, _controllerJsonOptions, _minimalApiJsonOptions);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ObjectTypeContract>()
            .Which.Type.Should().Be(type);

        var properties = (contract as ObjectTypeContract)!.Properties.ToArray();
        properties.Should().HaveCount(2);
        properties.Should().NotContain(x => x.Name == "fullName");
    }

    [Fact]
    public void GetContract_ShouldReturnPublicProperties_WhenDefaultOptions()
    {
        // Arrange
        var type = typeof(UserForProperties);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ObjectTypeContract>()
            .Which.Type.Should().Be(type);

        var properties = (contract as ObjectTypeContract)!.Properties.ToArray();
        properties.Should().HaveCount(3);
        properties.Should().Contain(x => x.Name == "id");
        properties.Should().Contain(x => x.Name == "theAge");
        properties.Should().Contain(x => x.Name == "fullName");
        properties.Should().NotContain(x => x.Name == "name");
    }

    [Fact]
    public void GetContract_ShouldIgnoreReadOnlyProperties_WhenOptionsSet()
    {
        // Arrange
        var type = typeof(UserForProperties);
        var options = new Microsoft.AspNetCore.Http.Json.JsonOptions
        {
            SerializerOptions =
            {
                IgnoreReadOnlyProperties = true
            }
        };
        _minimalApiJsonOptions.Value.Returns(options);
        _sut = new ContractFactory(_schemaGeneratorOptions, _controllerJsonOptions, _minimalApiJsonOptions);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ObjectTypeContract>()
            .Which.Type.Should().Be(type);

        var properties = (contract as ObjectTypeContract)!.Properties.ToArray();
        properties.Should().HaveCount(1);
        properties.Should().Contain(x => x.Name == "id");
        properties.Should().NotContain(x => x.Name == "fullName");
        properties.Should().NotContain(x => x.Name == "theAge");
    }
    
    [Fact]
    public void GetContract_ShouldReturnPropertyWithOrderAttributeFirst_WhenPropertyHasOrderAttribute()
    {
        // Arrange
        var type = typeof(UserForProperties);

        // Act
        var contract = _sut.GetContract(type, []);

        // Assert
        contract.Should().BeOfType<ObjectTypeContract>()
            .Which.Type.Should().Be(type);
        var properties = (contract as ObjectTypeContract)!.Properties.ToArray();
        properties[0].Name.Should().Be("fullName");
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
file enum TestEnum;

#pragma warning disable CS0169 // Field is never used
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
file class UserForFields
{
    [JsonInclude]
    [JsonPropertyName("dev")]
    private readonly bool _isDeveloper;

    [JsonPropertyName("fullName")]
    public readonly string? FullName;

    private string? _company;

    public string? Password;
}

file class UserForProperties
{
    public required string Id { get; set; }

    [JsonPropertyName("theAge")]
    public int Age { get; }

    private string? Name { get; init; }

    [JsonInclude]
    [JsonPropertyOrder(1)]
    private string FullName { get; } = null!;
}
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
#pragma warning restore CS0169 // Field is never used