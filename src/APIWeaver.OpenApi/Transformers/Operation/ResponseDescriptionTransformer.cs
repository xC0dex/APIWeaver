namespace APIWeaver;

internal sealed class ResponseDescriptionTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var responseDescriptions = context.Description.ActionDescriptor.EndpointMetadata.OfType<ResponseDescriptionAttribute>();
        foreach (var responseDescription in responseDescriptions)
        {
            if (operation.Responses.TryGetValue(responseDescription.StatusCode.ToString(), out var response))
            {
                response.Description = responseDescription.Description;
            }
        }

        return Task.CompletedTask;
    }
}