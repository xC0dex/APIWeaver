using System.Text.Json.Serialization;

namespace APIWeaver.Demo.Shared;

public class Book
{
    public required Guid BookId { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required int Pages { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BookType? BookType { get; set; }
}

// [JsonSerializable(typeof(Book))]
// [JsonSourceGenerationOptions(UseStringEnumConverter = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
// public partial class BookSerializerContext : JsonSerializerContext;