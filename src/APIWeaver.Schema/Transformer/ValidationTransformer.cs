using System.ComponentModel.DataAnnotations;

namespace APIWeaver.Schema.Transformer;

internal sealed class ValidationTransformer : IValidationTransformer
{
    public void AddValidationRequirements(OpenApiSchema schema, IEnumerable<ValidationAttribute> validationAttributes)
    {
        foreach (var validationAttribute in validationAttributes)
        {
            switch (validationAttribute)
            {
                case RangeAttribute rangeAttribute:
                    AddRangeAttribute(schema, rangeAttribute);
                    break;
                case MinLengthAttribute minLengthAttribute:
                    AddMinLengthAttribute(schema, minLengthAttribute);
                    break;
                case MaxLengthAttribute maxLengthAttribute:
                    AddMaxLengthAttribute(schema, maxLengthAttribute);
                    break;
                case StringLengthAttribute stringLengthAttribute:
                    AddStringLengthLengthAttribute(schema, stringLengthAttribute);
                    break;
                case RegularExpressionAttribute regularExpressionAttribute:
                    AddRegularExpressionAttribute(schema, regularExpressionAttribute);
                    break;
                case DataTypeAttribute dataTypeAttribute:
                    AddDataTypeAttribute(schema, dataTypeAttribute);
                    break;
#if NET8_0_OR_GREATER
                case LengthAttribute lengthAttribute:
                    AddLengthAttribute(schema, lengthAttribute);
                    break;
                case Base64StringAttribute:
                    AddBase64StringAttribute(schema);
                    break;
                case AllowedValuesAttribute allowedValuesAttribute:
                    AddAllowedValuesAttribute(schema, allowedValuesAttribute);
                    break;
#endif
            }
        }
    }

    private static void AddRangeAttribute(OpenApiSchema schema, RangeAttribute rangeAttribute)
    {
        if (decimal.TryParse(rangeAttribute.Maximum.ToString(), out var maximum))
        {
#if NET8_0_OR_GREATER
            if (rangeAttribute.MaximumIsExclusive)
            {
                maximum -= 1;
            }
#endif
            schema.Maximum = maximum;
        }

        if (decimal.TryParse(rangeAttribute.Minimum.ToString(), out var minimum))
        {
#if NET8_0_OR_GREATER
            if (rangeAttribute.MinimumIsExclusive)
            {
                minimum += 1;
            }
#endif
            schema.Minimum = minimum;
        }
    }

    private static void AddMinLengthAttribute(OpenApiSchema schema, MinLengthAttribute minLengthAttribute)
    {
        if (schema.IsType(OpenApiDataType.Array))
        {
            schema.MinItems = minLengthAttribute.Length;
        }
        else
        {
            schema.MinLength = minLengthAttribute.Length;
        }
    }

    private static void AddMaxLengthAttribute(OpenApiSchema schema, MaxLengthAttribute maxLengthAttribute)
    {
        if (schema.IsType(OpenApiDataType.Array))
        {
            schema.MaxItems = maxLengthAttribute.Length;
        }
        else
        {
            schema.MaxLength = maxLengthAttribute.Length;
        }
    }

    private static void AddStringLengthLengthAttribute(OpenApiSchema schema, StringLengthAttribute stringLengthAttribute)
    {
        schema.MinLength = stringLengthAttribute.MinimumLength;
        schema.MaxLength = stringLengthAttribute.MaximumLength;
    }

    private static void AddRegularExpressionAttribute(OpenApiSchema schema, RegularExpressionAttribute regularExpressionAttribute)
    {
        schema.Pattern = regularExpressionAttribute.Pattern;
    }

    private static void AddDataTypeAttribute(OpenApiSchema schema, DataTypeAttribute dataTypeAttribute)
    {
        var pattern = dataTypeAttribute.ToSchemaFormat();
        schema.Format = pattern ?? schema.Format;
    }

#if NET8_0_OR_GREATER
    private static void AddLengthAttribute(OpenApiSchema schema, LengthAttribute lengthAttribute)
    {
        if (schema.IsType(OpenApiDataType.Array))
        {
            schema.MinItems = lengthAttribute.MinimumLength;
            schema.MaxItems = lengthAttribute.MaximumLength;
        }
        else
        {
            schema.MinLength = lengthAttribute.MinimumLength;
            schema.MaxLength = lengthAttribute.MaximumLength;
        }
    }

    private static void AddBase64StringAttribute(OpenApiSchema schema)
    {
        schema.Format = "byte";
    }

    private static void AddAllowedValuesAttribute(OpenApiSchema schema, AllowedValuesAttribute allowedValuesAttribute)
    {
        if (schema.IsType(OpenApiDataType.String))
        {
            var values = allowedValuesAttribute.Values.OfType<string>();
            foreach (var value in values)
            {
                schema.Enum.Add(new OpenApiString(value));
            }

            return;
        }

        if (schema.IsType(OpenApiDataType.Integer))
        {
            var values = allowedValuesAttribute.Values.OfType<int>();
            foreach (var value in values)
            {
                schema.Enum.Add(new OpenApiInteger(value));
            }
        }
    }

#endif
}