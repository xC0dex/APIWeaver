namespace APIWeaver.Schema.Tests;

public class FieldInfoExtensionsTests
{
    [Fact]
    public void IsNullable_ShouldReturnTrue_ForNullableValueType()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.NullableInt))!;

        // Act
        var result = fieldInfo.IsNullable(true);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullable_ShouldReturnFalse_ForNonNullableValueType()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.NonNullableInt))!;

        // Act
        var result = fieldInfo.IsNullable(true);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullable_ShouldReturnTrue_ForNullableReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsTrue()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.NullableString))!;

        // Act
        var result = fieldInfo.IsNullable(true);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullable_ShouldReturnFalse_ForNonNullableReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsTrue()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.NonNullableString))!;

        // Act
        var result = fieldInfo.IsNullable(true);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullable_ShouldReturnTrue_ForReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsFalse()
    {
        // Arrange
        var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.NonNullableString))!;

        // Act
        var result = fieldInfo.IsNullable(false);

        // Assert
        result.Should().BeTrue();
    }
}

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
file class TestClass
{
    public int NonNullableInt;
    public string NonNullableString = null!;

    public int? NullableInt;
    public string? NullableString;
}

#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value