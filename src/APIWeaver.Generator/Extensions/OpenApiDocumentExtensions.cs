namespace APIWeaver;

internal static class OpenApiDocumentExtensions
{
    internal static Dictionary<string, Dictionary<OperationType, OpenApiOperation>> GetOperationsByTag(this OpenApiDocument document)
    {
        var operationsByTag = new Dictionary<string, Dictionary<OperationType, OpenApiOperation>>();
        foreach (var path in document.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                var tag = operation.Value.Tags.FirstOrDefault()?.Name ?? throw new InvalidOperationException("Operation must have at least one tag");
                if (!operationsByTag.TryGetValue(tag, out var value))
                {
                    value = [];
                    operationsByTag[tag] = value;
                }

                value.Add(operation.Key, operation.Value);
            }
        }

        return operationsByTag;
    }
}