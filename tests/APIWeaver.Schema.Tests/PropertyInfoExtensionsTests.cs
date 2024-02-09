namespace APIWeaver.Schema.Tests;

public class PropertyInfoExtensionsTests
{
    [Fact]
    public void IsNullable_ShouldReturnTrue_ForNullableValueType()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetProperty(nameof(TestClass.NullableInt))!;

        // Act
        var result = fieldInfo.IsNullable(true);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullable_ShouldReturnFalse_ForNonNullableValueType()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetProperty(nameof(TestClass.NonNullableInt))!;

        // Act
        var result = fieldInfo.IsNullable(true);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullable_ShouldReturnTrue_ForNullableReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsTrue()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetProperty(nameof(TestClass.NullableString))!;

        // Act
        var result = fieldInfo.IsNullable(true);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullable_ShouldReturnFalse_ForNonNullableReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsTrue()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetProperty(nameof(TestClass.NonNullableString))!;

        // Act
        var result = fieldInfo.IsNullable(true);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullable_ShouldReturnTrue_ForReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsFalse()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetProperty(nameof(TestClass.NonNullableString))!;

        // Act
        var result = fieldInfo.IsNullable(false);

        // Assert
        result.Should().BeTrue();
    }
}

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
file class TestClass
{
    public int? NullableInt { get; set; }

    public int NonNullableInt { get; set; }
    public string? NullableString { get; set; }
    public string NonNullableString { get; set; } = null!;
}

#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value