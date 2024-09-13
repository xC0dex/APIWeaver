namespace APIWeaver;

internal static class OpenApiDocumentExtensions
{
    internal static Dictionary<string, List<OpenApiOperation>> GetOperationsByTag(this OpenApiDocument document)
    {
        var operationsByTag = new Dictionary<string, List<OpenApiOperation>>();
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

                value.Add(operation.Value);
            }
        }

        return operationsByTag;
    }
}