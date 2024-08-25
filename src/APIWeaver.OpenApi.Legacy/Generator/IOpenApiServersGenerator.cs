namespace APIWeaver.OpenApi.Generator;

internal interface IOpenApiServersGenerator
{
    Task<IList<OpenApiServer>> GenerateServersAsync(string url, CancellationToken cancellationToken);
}