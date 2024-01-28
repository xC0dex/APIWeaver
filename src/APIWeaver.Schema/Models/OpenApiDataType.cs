namespace APIWeaver.Schema.Models;

/// <summary>
/// Enum representing the possible data types in an OpenAPI schema.
/// </summary>
public enum OpenApiDataType
{
    /// <summary>
    /// Represents a string data type.
    /// </summary>
    String,

    /// <summary>
    /// Represents a number data type.
    /// </summary>
    Number,

    /// <summary>
    /// Represents an integer data type.
    /// </summary>
    Integer,

    /// <summary>
    /// Represents a boolean data type.
    /// </summary>
    Boolean,

    /// <summary>
    /// Represents an array data type.
    /// </summary>
    Array,

    /// <summary>
    /// Represents an object data type.
    /// </summary>
    Object
}