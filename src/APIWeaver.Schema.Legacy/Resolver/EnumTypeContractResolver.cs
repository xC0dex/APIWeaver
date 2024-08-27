namespace APIWeaver.Schema.Resolver;

internal sealed class EnumTypeContractResolver(ISchemaRepository schemaRepository) : IContractResolver<EnumTypeContract>
{
    public OpenApiSchema GenerateSchema(EnumTypeContract contract)
    {
        var schemaReference = schemaRepository.GetSchemaReference(contract.Type);
        if (schemaReference is not null)
        {
            return schemaReference;
        }

        var schema = new OpenApiSchema
        {
            Type = contract.PrimitiveTypeContract.PrimitiveTypeDefinition.Type.ToStringFast(),
            Format = contract.PrimitiveTypeContract.PrimitiveTypeDefinition.Format,
            Enum = []
        };

        var values = contract.Type.GetEnumValues();
        var serializeAsString = contract.PrimitiveTypeContract.PrimitiveTypeDefinition.Type == OpenApiDataType.String;
        if (serializeAsString)
        {
            foreach (var value in values)
            {
                schema.Enum.Add(new OpenApiString(value.ToString()));
            }
        }
        else
        {
            foreach (var value in values)
            {
                schema.Enum.Add(new OpenApiInteger((int) value));
            }
        }

        return schemaRepository.AddOpenApiSchema(contract.Type, schema);
    }
}