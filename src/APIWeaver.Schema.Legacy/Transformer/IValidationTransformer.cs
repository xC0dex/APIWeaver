using System.ComponentModel.DataAnnotations;

namespace APIWeaver.Schema.Transformer;

internal interface IValidationTransformer
{
    void AddValidationRequirements(OpenApiSchema schema, IEnumerable<ValidationAttribute> validationAttributes);
}