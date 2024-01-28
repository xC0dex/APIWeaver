using System.ComponentModel.DataAnnotations;

namespace APIWeaver.Schema.Resolver;

internal sealed class PrimitiveTypeContractResolver(IValidationTransformer validationTransformer) : IContractResolver<PrimitiveTypeContract>
{
    public OpenApiSchema GenerateSchema(PrimitiveTypeContract contract)
    {
        var schema = new OpenApiSchema
        {
            Type = contract.PrimitiveTypeDefinition.Type.ToStringFast(),
            Format = contract.PrimitiveTypeDefinition.Format
        };
        validationTransformer.AddValidationRequirements(schema, contract.CustomAttributes.OfType<ValidationAttribute>());

        return schema;
    }
}