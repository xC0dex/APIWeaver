using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Schema.Tests;

public class ValidationTransformerTests
{
    private readonly ValidationTransformer _sut = new();

    [Fact]
    public void AddValidationRequirements_ShouldAddRequirements_WhenRangeAttribute()
    {
        // Arrange
        ValidationAttribute[] attributes = [new RangeAttribute(69, 420)];
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, attributes);

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
        ValidationAttribute[] attributes = [attribute];
        var schema = new OpenApiSchema();

        // Act
        _sut.AddValidationRequirements(schema, attributes);

        // Assert
        schema.Minimum.Should().Be(70);
        schema.Maximum.Should().Be(419);
    }
}