using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Extensions;

namespace APIWeaver;

internal sealed class RequestBodyParameterNameTransformer : IOpenApiOperationTransformer
{
    private const string NameKey = "x-name";

    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (operation.RequestBody is not null)
        {
            var requestBodyParameter = context.Description.ParameterDescriptions.FirstOrDefault(p => p.Source == BindingSource.Body);
            if (requestBodyParameter is not null)
            {
                operation.RequestBody.AddExtension(NameKey, new OpenApiString(requestBodyParameter.Name));
            }
        }

        return Task.CompletedTask;
    }
}