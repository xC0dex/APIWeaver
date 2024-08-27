using System.Text.Json.Serialization;

namespace APIWeaver.Demo.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookType
{
    Newsletter,
    Documentation
}