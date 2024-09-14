using System.Text.Json.Serialization;

namespace APIWeaver.Demo.Client;

[JsonSerializable(typeof(UserDto))]
[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class UserDtoSerializerContext : JsonSerializerContext;