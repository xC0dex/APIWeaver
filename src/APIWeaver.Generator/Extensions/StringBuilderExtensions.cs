using System.Text;

namespace APIWeaver;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendCode(this StringBuilder builder, string value, ushort indent = 0)
    {
        for (var i = 0; i < indent; i++)
        {
            builder.Append("    ");
        }

        return builder.AppendLine(value);
    }
    
    public static StringBuilder AppendCode(this StringBuilder builder, ushort indent = 0)
    {
        for (var i = 0; i < indent; i++)
        {
            builder.Append("    ");
        }

        return builder.AppendLine();
    }
}