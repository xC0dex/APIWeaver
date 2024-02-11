using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace APIWeaver.Schema.Resolver;

internal sealed class ObjectTypeContractResolver(ISchemaGenerator schemaGenerator, IValidationTransformer validationTransformer, ISchemaRepository schemaRepository) : IContractResolver<ObjectTypeContract>
{
    public OpenApiSchema GenerateSchema(ObjectTypeContract contract)
    {
        var schemaReference = schemaRepository.GetSchemaReference(contract.Type);
        if (schemaReference is not null)
        {
            return schemaReference;
        }

        var schema = new OpenApiSchema
        {
            Type = OpenApiDataType.Object.ToStringFast(),
            Properties = new Dictionary<string, OpenApiSchema>(),
            AdditionalPropertiesAllowed = false,
            Required = new HashSet<string>()
        };
        GenerateSchemaForProperties(schema, contract.Properties);

        validationTransformer.AddValidationRequirements(schema, contract.CustomAttributes.OfType<ValidationAttribute>());
        return schemaRepository.AddOpenApiSchema(contract.Type, schema);
    }

    private void GenerateSchemaForProperties(OpenApiSchema schema, IEnumerable<PropertyContract> properties)
    {
        foreach (var propertyContract in properties)
        {
#if NET7_0_OR_GREATER
            var requiredMember = propertyContract.CustomAttributes.OfType<RequiredMemberAttribute>().Any();
            if (requiredMember)
            {
                schema.Required.Add(propertyContract.Name);
            }
#endif

            List<Action<OpenApiSchema>> schemaChanges = [];

            schemaChanges.AddWhen(propertyContract.Nullable, s => s.Nullable = true);
            schemaChanges.AddWhen(propertyContract.CustomAttributes.OfType<ObsoleteAttribute>().Any(), s => s.Deprecated = true);

            var requiredAttribute = propertyContract.CustomAttributes.OfType<RequiredAttribute>().FirstOrDefault();
            if (requiredAttribute is not null)
            {
                schema.Required.Add(propertyContract.Name);

                schemaChanges.AddWhen(propertyContract.Type == typeof(string), s =>
                {
                    s.MinLength = requiredAttribute.AllowEmptyStrings ? s.MinLength : 1;
                    s.Nullable = requiredAttribute.AllowEmptyStrings && s.Nullable;
                });
            }


            var propertySchema = schemaGenerator.GenerateSchema(propertyContract.Type, propertyContract.CustomAttributes);
            if (schemaChanges.Count > 0)
            {
                var requireOneOfSchema = propertySchema.Reference is not null;
                if (requireOneOfSchema)
                {
                    propertySchema = schemaRepository.GetOneOfSchemaReference(Nullable.GetUnderlyingType(propertyContract.Type) ?? propertyContract.Type);
                }

                schemaChanges.Apply(propertySchema);
            }

            schema.Properties.Add(propertyContract.Name, propertySchema);
        }
    }
}