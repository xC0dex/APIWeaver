namespace APIWeaver.Schema.Tests;

public class PropertyInfoExtensionsTests
{
    [Fact]
    public void IsNullable_ShouldReturnTrue_ForNullableValueType()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.NullableInt))!;

        // Act
        var result = propertyInfo.IsNullable(true);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullable_ShouldReturnFalse_ForNonNullableValueType()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.NonNullableInt))!;

        // Act
        var result = propertyInfo.IsNullable(true);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullable_ShouldReturnTrue_ForNullableReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsTrue()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.NullableString))!;

        // Act
        var result = propertyInfo.IsNullable(true);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullable_ShouldReturnFalse_ForNonNullableReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsTrue()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.NonNullableString))!;

        // Act
        var result = propertyInfo.IsNullable(true);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullable_ShouldReturnTrue_ForReferenceType_WhenCheckNullableAnnotationForReferenceTypesIsFalse()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.NonNullableString))!;

        // Act
        var result = propertyInfo.IsNullable(false);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsPublic_ShouldReturnTrue_WhenPropertyIsPublic()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.NonNullableString))!;

        // Act
        var result = propertyInfo.IsPublic();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsPublic_ShouldReturnFalse_WhenPropertyHasNoGetter()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.PropertyWithoutGetter))!;

        // Act
        var result = propertyInfo.IsPublic();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsReadonly_ShouldReturnTrue_WhenPropertyIsReadonly()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.ReadOnlyProperty))!;

        // Act
        var result = propertyInfo.IsReadonly();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsReadonly_ShouldReturnFalse_WhenPropertyHasSetter()
    {
        // Arrange
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.PropertyWithoutGetter))!;

        // Act
        var result = propertyInfo.IsReadonly();

        // Assert
        result.Should().BeFalse();
    }
}

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
file class TestClass
{
    private int _propertyWithoutGetter;
    public int? NullableInt { get; set; }

    public int NonNullableInt { get; set; }
    public string? NullableString { get; set; }
    public string NonNullableString { get; set; } = null!;

    public int PropertyWithoutGetter
    {
        set => _propertyWithoutGetter = value;
    }

    public int ReadOnlyProperty { get; }
}

#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value