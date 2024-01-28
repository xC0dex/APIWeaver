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
}