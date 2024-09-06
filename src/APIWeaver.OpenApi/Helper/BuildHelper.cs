namespace APIWeaver;

/// <summary>
/// Helper class for build-related operations.
/// </summary>
public static class BuildHelper
{
    static BuildHelper()
    {
        var args = Environment.GetCommandLineArgs();
        IsGenerationContext = Array.Exists(args, x => x.AsSpan().EndsWith("GetDocument.Insider.dll"));
    }

    /// <summary>
    /// Gets a value indicating whether the current invocation is for document generation context.
    /// </summary>
    public static bool IsGenerationContext { get; private set; }
}