namespace APIWeaver;

/// <summary>
/// Helper class for build-related operations.
/// </summary>
public static class BuildHelper
{
    /// <summary>
    /// Gets a value indicating whether the current invocation is for document generation.
    /// </summary>
    public static bool IsGetDocumentInvoke { get; private set; }

    static BuildHelper()
    {
        var args = Environment.GetCommandLineArgs();
        IsGetDocumentInvoke = Array.Exists(args, x => x.AsSpan().EndsWith("GetDocument.Insider.dll"));
    }
}