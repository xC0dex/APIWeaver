using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace APIWeaver.Schema.Extensions;

/// <summary>
/// Extension methods for setting up APIWeaver Schema services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds APIWeaver Schema services to <see cref="IServiceCollection" />.
    /// </summary>
    public static IServiceCollection AddApiWeaverSchema(this IServiceCollection services)
    {
        services.TryAddScoped<IContractResolverFactory, ContractResolverFactory>();

        services.TryAddScoped<IContractResolver<EnumTypeContract>, EnumTypeContractResolver>();
        services.TryAddScoped<IContractResolver<ArrayTypeContract>, ArrayTypeContractResolver>();
        services.TryAddScoped<IContractResolver<PrimitiveTypeContract>, PrimitiveTypeContractResolver>();
        services.TryAddScoped<IContractResolver<DictionaryTypeContract>, DictionaryTypeContractResolver>();
        services.TryAddScoped<IContractResolver<ObjectTypeContract>, ObjectTypeContractResolver>();
        services.TryAddScoped<IContractResolver<UndefinedTypeContract>, UndefinedTypeContractResolver>();

        services.TryAddScoped<IValidationTransformer, ValidationTransformer>();
        services.TryAddScoped<ISchemaGenerator, SchemaGenerator>();
        services.TryAddScoped<IContractFactory, ContractFactory>();

        services.TryAddScoped<ISchemaRepository, SchemaRepository>();
        return services;
    }
}