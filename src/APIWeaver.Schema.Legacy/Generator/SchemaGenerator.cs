using Microsoft.Extensions.Options;

namespace APIWeaver.Schema.Generator;

internal sealed class SchemaGenerator(
    IContractFactory contractFactory,
    IContractResolverFactory contractResolverFactory,
    IServiceProvider serviceProvider,
    IOptions<OpenApiSchemaGeneratorOptions> schemaGeneratorOptions)
    : ISchemaGenerator
{
    public async Task<OpenApiSchema> GenerateSchemaAsync(Type type, IEnumerable<Attribute> customAttributes, CancellationToken cancellationToken)
    {
        var attributes = customAttributes.ToArray();
        var schema = GenerateSchema(type, attributes);
        cancellationToken.ThrowIfCancellationRequested();
        var schemaContext = new SchemaContext(schema, type, attributes, serviceProvider, cancellationToken);
        foreach (var schemaTransformer in schemaGeneratorOptions.Value.SchemaTransformers)
        {
            var task = schemaTransformer.TransformAsync(schemaContext);
            if (!task.IsCompleted)
            {
                await task;
            }
        }

        return schema;
    }

    public OpenApiSchema GenerateSchema(Type type, IEnumerable<Attribute> customAttributes)
    {
        var concreteType = Nullable.GetUnderlyingType(type) ?? type;
        var contract = contractFactory.GetContract(concreteType, customAttributes);
        return contract switch
        {
            PrimitiveTypeContract primitiveTypeContract => contractResolverFactory.GetContractResolver<PrimitiveTypeContract>().GenerateSchema(primitiveTypeContract),
            EnumTypeContract enumTypeContract => contractResolverFactory.GetContractResolver<EnumTypeContract>().GenerateSchema(enumTypeContract),
            ArrayTypeContract arrayTypeContract => contractResolverFactory.GetContractResolver<ArrayTypeContract>().GenerateSchema(arrayTypeContract),
            DictionaryTypeContract dictionaryTypeContract => contractResolverFactory.GetContractResolver<DictionaryTypeContract>().GenerateSchema(dictionaryTypeContract),
            ObjectTypeContract objectTypeContract => contractResolverFactory.GetContractResolver<ObjectTypeContract>().GenerateSchema(objectTypeContract),
            UndefinedTypeContract undefinedTypeContract => contractResolverFactory.GetContractResolver<UndefinedTypeContract>().GenerateSchema(undefinedTypeContract),
            _ => throw new UnknownContractException()
        };
    }
}