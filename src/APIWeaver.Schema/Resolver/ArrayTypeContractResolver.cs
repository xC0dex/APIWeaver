namespace APIWeaver.Schema.Resolver;

internal sealed class ArrayTypeContractResolver(ISchemaGenerator schemaGenerator) : IContractResolver<ArrayTypeContract>
{
    public OpenApiSchema GenerateSchema(ArrayTypeContract contract)
    {
        var schema = new OpenApiSchema
        {
            Type = OpenApiDataType.Array.ToStringFast(),
            UniqueItems = contract.UniqueItems,
            Items = schemaGenerator.GenerateSchema(contract.ItemType, contract.ItemType.GetCustomAttributes())
        };
        return schema;
    }
}