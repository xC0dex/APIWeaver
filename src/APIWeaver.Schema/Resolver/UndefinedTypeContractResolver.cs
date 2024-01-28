namespace APIWeaver.Schema.Resolver;

internal sealed class UndefinedTypeContractResolver : IContractResolver<UndefinedTypeContract>
{
    public OpenApiSchema GenerateSchema(UndefinedTypeContract contract) => new();
}