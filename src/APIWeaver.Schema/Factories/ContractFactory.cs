using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace APIWeaver.Schema.Factories;

internal sealed class ContractFactory(
    IOptions<OpenApiSchemaGeneratorOptions> schemaGeneratorOptions,
    IOptions<JsonOptions> controllerJsonOptions,
    IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions> minimalApiJsonOptions)
    : IContractFactory
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = schemaGeneratorOptions.Value.JsonOptionsSource == JsonOptionsSource.MinimalApiOptions
        ? minimalApiJsonOptions.Value.SerializerOptions
        : controllerJsonOptions.Value.JsonSerializerOptions;

    public IContract GetContract(Type type, IEnumerable<Attribute> customAttributes)
    {
        if (TypeHelper.PrimitiveTypes.TryGetValue(type, out var typeDefinition))
        {
            return new PrimitiveTypeContract(typeDefinition, customAttributes);
        }


        if (type.IsUndefined())
        {
            return new UndefinedTypeContract();
        }

        if (type.IsEnum)
        {
            var attributes = customAttributes.ToArray();
            var serializeAsString = type.HasStringEnumConverterAttribute() || attributes.ContainsStringEnumConverterAttribute() || _jsonSerializerOptions.Converters.OfType<JsonStringEnumConverter>().Any();
            var primitiveType = serializeAsString ? typeof(string) : typeof(int);
            var primitiveTypeContract = new PrimitiveTypeContract(TypeHelper.PrimitiveTypes[primitiveType], attributes);
            return new EnumTypeContract(primitiveTypeContract, type);
        }

        if (type.IsDictionary())
        {
            var genericArguments = type.GenericTypeArguments;
            var isGeneric = genericArguments.Length > 1;
            var valueType = isGeneric ? genericArguments[1] : typeof(object);
            return new DictionaryTypeContract(valueType);
        }

        if (type.IsEnumerable())
        {
            var genericType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault() ?? typeof(object);
            return new ArrayTypeContract(genericType, type.IsSet(), customAttributes);
        }

        var properties = GetProperties(type);

        return new ObjectTypeContract(type, properties, customAttributes);
    }

    private IEnumerable<PropertyContract> GetProperties(Type type)
    {
        var publicProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p =>
        {
            var attribute = p.GetCustomAttribute<JsonIgnoreAttribute>();
            return attribute is null || attribute.Condition != JsonIgnoreCondition.Always;
        });
        var nonPublicProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Where(p => p.GetCustomAttribute<JsonIncludeAttribute>() is not null);

        var visibleProperties = publicProperties.Concat(nonPublicProperties).Select(propertyInfo =>
        {
            var attribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
            var name = attribute?.Name ?? _jsonSerializerOptions.PropertyNamingPolicy?.ConvertName(propertyInfo.Name) ?? propertyInfo.Name;

            var isNullable = propertyInfo.IsNullable(schemaGeneratorOptions.Value.NullableAnnotationForReferenceTypes);
            const bool isReadonly = false;
            const bool isWriteOnly = false;
            return new PropertyContract(propertyInfo.PropertyType, name, isNullable, isReadonly, isWriteOnly, propertyInfo.GetCustomAttributes());
        });

        var fields = GetPropertiesFromFields(type);

        return visibleProperties.Concat(fields);
    }

    private IEnumerable<PropertyContract> GetPropertiesFromFields(Type type)
    {
        return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(fieldInfo =>
        {
            var compilerGenerated = fieldInfo.GetCustomAttribute<CompilerGeneratedAttribute>();

            // Ignore backing fields
            if (compilerGenerated is not null)
            {
                return false;
            }

            var jsonInclude = fieldInfo.GetCustomAttribute<JsonIncludeAttribute>();

            // Always include fields with JsonIncludeAttribute
            if (jsonInclude is not null)
            {
                return true;
            }

            // Ignore fields if IncludeFields is false
            if (!_jsonSerializerOptions.IncludeFields)
            {
                return false;
            }

            // Ignore readonly fields if IgnoreReadOnlyFields is true
            if (fieldInfo.IsInitOnly && _jsonSerializerOptions.IgnoreReadOnlyFields)
            {
                return false;
            }

            var jsonIgnore = fieldInfo.GetCustomAttribute<JsonIgnoreAttribute>();
            var ignoreField = jsonIgnore is not null && jsonIgnore.Condition == JsonIgnoreCondition.Always;

            // Include public fields
            return !ignoreField && fieldInfo.IsPublic;
        }).Select(fieldInfo =>
        {
            var attribute = fieldInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
            var name = attribute?.Name ?? _jsonSerializerOptions.PropertyNamingPolicy?.ConvertName(fieldInfo.Name) ?? fieldInfo.Name;
            var isNullable = fieldInfo.IsNullable(schemaGeneratorOptions.Value.NullableAnnotationForReferenceTypes);
            var isReadonly = fieldInfo.IsInitOnly;
            const bool isWriteOnly = false;
            return new PropertyContract(fieldInfo.FieldType, name, isNullable, isReadonly, isWriteOnly, fieldInfo.GetCustomAttributes());
        });
    }
}