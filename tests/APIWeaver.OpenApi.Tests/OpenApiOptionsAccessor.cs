using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.OpenApi;

namespace APIWeaver.OpenApi.Tests;

internal static class OpenApiOptionsAccessor
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "DocumentTransformers")]
    internal static extern ref List<IOpenApiDocumentTransformer> GetDocumentTransformers(OpenApiOptions options);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "OperationTransformers")]
    internal static extern ref List<IOpenApiOperationTransformer> GetOperationTransformers(OpenApiOptions options);
}