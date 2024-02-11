using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Schema.Tests;

public class ValidationTransformerTests
{
    private readonly ValidationTransformer _sut = new();

    [Fact]
    public void AddValidationRequirements_ShouldAddRequirements_WhenRangeAttribute()
    {
        // Arrange
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [new RangeAttribute(69, 420)]);

        // Assert
        schema.Minimum.Should().Be(69);
        schema.Maximum.Should().Be(420);
    }

    [Fact]
    public void AddValidationRequirements_ShouldAdjustRange_WhenRangeAttributeIsExclusive()
    {
        // Arrange
        var attribute = new RangeAttribute(69, 420)
        {
            MaximumIsExclusive = true,
            MinimumIsExclusive = true
        };
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.Minimum.Should().Be(70);
        schema.Maximum.Should().Be(419);
    }

    [Fact]
    public void AddValidationRequirements_ShouldSetMinLength_WhenAttributeIsMinLength()
    {
        // Arrange
        var attribute = new MinLengthAttribute(69);
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.MinLength.Should().Be(69);
    }

    [Fact]
    public void AddValidationRequirements_ShouldSetMaxLength_WhenAttributeIsMaxLength()
    {
        // Arrange
        var attribute = new MaxLengthAttribute(420);
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.MaxLength.Should().Be(420);
    }

    [Fact]
    public void AddValidationRequirements_ShouldSetMinItems_WhenAttributeIsMinLengthAndArrayType()
    {
        // Arrange
        var attribute = new MinLengthAttribute(69);
        var schema = new OpenApiSchema
        {
            Type = OpenApiDataType.Array.ToStringFast()
        };

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.MinItems.Should().Be(69);
    }

    [Fact]
    public void AddValidationRequirements_ShouldSetMaxItems_WhenAttributeIsMaxLengthAndArrayType()
    {
        // Arrange
        var attribute = new MaxLengthAttribute(420);
        var schema = new OpenApiSchema
        {
            Type = OpenApiDataType.Array.ToStringFast()
        };

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.MaxItems.Should().Be(420);
    }

    [Fact]
    public void AddValidationRequirements_ShouldSetLengthConstrain_WhenAttributeIsStringLength()
    {
        // Arrange
        var attribute = new StringLengthAttribute(420)
        {
            MinimumLength = 69
        };
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.MinLength.Should().Be(69);
        schema.MaxLength.Should().Be(420);
    }

    [Fact]
    public void AddValidationRequirements_ShouldSetPattern_WhenAttributeIsRegex()
    {
        // Arrange
        var attribute = new RegularExpressionAttribute(@"^\w");
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.Pattern.Should().Be(@"^\w");
    }

    [Theory]
    [InlineData(DataType.Date, "date")]
    [InlineData(DataType.Time, "time")]
    [InlineData(DataType.DateTime, "date-time")]
    [InlineData(DataType.Duration, "duration")]
    [InlineData(DataType.PhoneNumber, "tel")]
    [InlineData(DataType.Currency, "currency")]
    [InlineData(DataType.Text, "text")]
    [InlineData(DataType.Html, "html")]
    [InlineData(DataType.MultilineText, "multiline")]
    [InlineData(DataType.EmailAddress, "email")]
    [InlineData(DataType.Password, "password")]
    [InlineData(DataType.Url, "uri")]
    [InlineData(DataType.ImageUrl, "uri")]
    [InlineData(DataType.CreditCard, "credit-card")]
    [InlineData(DataType.PostalCode, "postal-code")]
    [InlineData(DataType.Upload, "binary")]
    public void AddValidationRequirements_ShouldSetFormat_WhenAttributeIsDataType(DataType dataType, string expectedFormat)
    {
        // Arrange
        var attribute = new DataTypeAttribute(dataType);
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.Format.Should().Be(expectedFormat);
    }

    [Fact]
    public void AddValidationRequirements_ShouldUseCustomFormat_WhenAttributeDataTypeIsCustom()
    {
        // Arrange
        var attribute = new DataTypeAttribute("custom");
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.Format.Should().Be("custom");
    }

    [Fact]
    public void AddValidationRequirements_ShouldNotChangeFormat_WhenAttributeIsDataTypeAndUnknown()
    {
        // Arrange
        var attribute = new DataTypeAttribute((DataType) 999);
        var schema = new OpenApiSchema
        {
            Format = "existing"
        };

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.Format.Should().Be("existing");
    }
    
    [Fact]
    public void AddValidationRequirements_ShouldNotLengthConstrains_WhenAttributeIsLength()
    {
        // Arrange
        var attribute = new LengthAttribute(69, 420);
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.MinLength.Should().Be(69);
        schema.MaxLength.Should().Be(420);
    }
    
    [Fact]
    public void AddValidationRequirements_ShouldNotLengthConstrains_WhenAttributeIsLengthAndArrayType()
    {
        // Arrange
        var attribute = new LengthAttribute(69, 420);
        var schema = new OpenApiSchema
        {
            Type = OpenApiDataType.Array.ToStringFast()
        };

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.MinItems.Should().Be(69);
        schema.MaxItems.Should().Be(420);
    }
    
    [Fact]
    public void AddValidationRequirements_ShouldAddByteFormat_WhenAttributeIsBase64()
    {
        // Arrange
        var attribute = new Base64StringAttribute();
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.Format.Should().Be("byte");
    }
    
    [Fact]
    public void AddValidationRequirements_ShouldAddEnumWithStringValues_WhenAllowedValuesAttributeWithStringValues()
    {
        // Arrange
        var attribute = new AllowedValuesAttribute("C", "#");
        var schema = new OpenApiSchema
        {
            Type = OpenApiDataType.String.ToStringFast()
        };

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.Enum.Count.Should().Be(2);
        var values = schema.Enum.Cast<OpenApiString>().ToArray();
        values[0].Value.Should().Be("C");
        values[1].Value.Should().Be("#");
    }
    
    [Fact]
    public void AddValidationRequirements_ShouldAddEnumWithIntegerValues_WhenAllowedValuesAttributeWithIntValues()
    {
        // Arrange
        var attribute = new AllowedValuesAttribute(1, 2);
        var schema = new OpenApiSchema
        {
            Type = OpenApiDataType.Integer.ToStringFast()
        };

        // Act
        _sut.AddValidationRequirements(schema, [attribute]);

        // Assert
        schema.Enum.Count.Should().Be(2);
        var values = schema.Enum.Cast<OpenApiInteger>().ToArray();
        values[0].Value.Should().Be(1);
        values[1].Value.Should().Be(2);
    }
}