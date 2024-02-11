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
        if (schema.Type == OpenApiDataType.Array.ToStringFast())
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
        if (schema.Type == OpenApiDataType.Array.ToStringFast())
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
}