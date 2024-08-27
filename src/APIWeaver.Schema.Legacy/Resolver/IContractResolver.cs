namespace APIWeaver.Schema.Resolver;

internal interface IContractResolver<in TContract>
{
    OpenApiSchema GenerateSchema(TContract contract);
}