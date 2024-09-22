namespace APIWeaver;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendIndent(this StringBuilder builder, string value, int indent = 0)
    {
        for (var i = 0; i < indent; i++) builder.Append("    ");

        return builder.Append(value);
    }

    public static StringBuilder AppendIndent(this StringBuilder builder, int indent = 0)
    {
        for (var i = 0; i < indent; i++) builder.Append("    ");

        return builder;
    }

    public static StringBuilder AppendIndentLine(this StringBuilder builder, string value, int indent = 0)
    {
        for (var i = 0; i < indent; i++) builder.Append("    ");

        return builder.AppendLine(value);
    }

    public static StringBuilder AppendIndentLine(this StringBuilder builder, int indent = 0)
    {
        for (var i = 0; i < indent; i++) builder.Append("    ");

        return builder.AppendLine();
    }
}