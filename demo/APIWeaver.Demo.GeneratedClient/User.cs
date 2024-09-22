namespace APIWeaver.Demo.GeneratedClient;

internal sealed class User
{
    public required Guid UserId { get; set; }

    public string? Name { get; set; }

    public int Age { get; set; }
}

// [JsonSerializable(typeof(User))]
// [JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
// internal sealed partial class UserSerializerContext : JsonSerializerContext;