namespace APIWeaver.Schema.Resolver;

internal sealed class DictionaryTypeContractResolver(ISchemaGenerator schemaGenerator) : IContractResolver<DictionaryTypeContract>
{
    public OpenApiSchema GenerateSchema(DictionaryTypeContract contract)
    {
        var schema = new OpenApiSchema
        {
            Type = OpenApiDataType.Object.ToStringFast(),
            AdditionalProperties = schemaGenerator.GenerateSchema(contract.ValueType, contract.ValueType.GetCustomAttributes()),
            AdditionalPropertiesAllowed = true
        };

        return schema;
    }
}