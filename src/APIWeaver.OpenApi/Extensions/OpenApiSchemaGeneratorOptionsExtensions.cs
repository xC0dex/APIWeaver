namespace APIWeaver;

/// <summary>
/// Extension methods for <see cref="OpenApiSchemaGeneratorOptions" />-
/// </summary>
public static class OpenApiSchemaGeneratorOptionsExtensions
{
    /// <summary>
    /// Sets the <see cref="OpenApiSchemaGeneratorOptions.JsonOptionsSource" /> property.
    /// </summary>
    /// <param name="options"><see cref="OpenApiSchemaGeneratorOptions" />.</param>
    /// <param name="jsonOptionsSource">The JsonOptionsSource value to set.</param>
    public static OpenApiSchemaGeneratorOptions WithJsonOptionsSource(this OpenApiSchemaGeneratorOptions options, JsonOptionsSource jsonOptionsSource)
    {
        options.JsonOptionsSource = jsonOptionsSource;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="OpenApiSchemaGeneratorOptions.NullableAnnotationForReferenceTypes" /> property.
    /// </summary>
    /// <param name="options"><see cref="OpenApiSchemaGeneratorOptions" />.</param>
    /// <param name="nullableAnnotationForReferenceTypes">The NullableAnnotationForReferenceTypes value to set.</param>
    public static OpenApiSchemaGeneratorOptions WithNullableAnnotationForReferenceTypes(this OpenApiSchemaGeneratorOptions options, bool nullableAnnotationForReferenceTypes)
    {
        options.NullableAnnotationForReferenceTypes = nullableAnnotationForReferenceTypes;
        return options;
    }
}