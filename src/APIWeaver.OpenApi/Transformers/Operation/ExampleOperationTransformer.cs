using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APIWeaver;

internal sealed class ExampleOperationTransformer: IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.HasExample())
        {
            if (operation.RequestBody is not null)
            {
                var requestBodyParameter = context.Description.ParameterDescriptions.FirstOrDefault(p => p.Source == BindingSource.Body);
                if (requestBodyParameter is not null)
                {
                    var type = requestBodyParameter.Type;
                }
            }

        }
        return Task.CompletedTask;
    }
}