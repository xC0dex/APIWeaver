using System.Text;

namespace APIWeaver;

internal sealed class MethodSourceCodeBuilder
{
    private int _baseIndent = 1;

    internal string Build(List<Method> methods)
    {
        var builder = new StringBuilder();

        for (var i = 0; i < methods.Count; i++)
        {
            builder.Append(Build(methods[i]));
            if (i < methods.Count - 1)
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }

    private string Build(Method method)
    {
        var builder = new StringBuilder();
        builder.AppendLine();
        builder.Append(BuildSignature(method));
        builder.AppendIndentLine("{", _baseIndent);
        _baseIndent++;
        builder.Append(BuildBody(method));
        _baseIndent--;
        builder.AppendIndent("}", _baseIndent);
        return builder.ToString();
    }

    private string BuildSignature(Method method)
    {
        var builder = new StringBuilder();
        builder.AppendIndent("public async ", _baseIndent);
        builder.AppendIndent(BuildReturnType(method));
        builder.AppendIndent($" {method.Name}");
        builder.Append('<');
        for (var i = 0; i < method.ResponseTypes.Length; i++)
        {
            builder.Append($"T{method.ResponseTypes[i]}");
            if (i < method.ResponseTypes.Length - 1)
            {
                builder.Append(", ");
            }
        }

        builder.Append('>');
        builder.Append('(');
        builder.Append(')');
        builder.AppendLine();
        return builder.ToString();
    }

    private static string BuildReturnType(Method method)
    {
        var builder = new StringBuilder();
        builder.Append("Task");
        builder.Append('<');
        builder.Append("Response");
        builder.Append('<');
        for (var i = 0; i < method.ResponseTypes.Length; i++)
        {
            builder.Append($"T{method.ResponseTypes[i]}");
            if (i < method.ResponseTypes.Length - 1)
            {
                builder.Append(", ");
            }
        }

        builder.Append('>');
        builder.Append('>');
        return builder.ToString();
    }

    private string BuildBody(Method method)
    {
        var builder = new StringBuilder();
        builder.AppendIndentLine("using var request = new HttpRequestMessage();", _baseIndent);
        builder.AppendIndentLine($"request.Method = HttpMethod.{method.HttpMethod};", _baseIndent);
        builder.AppendIndentLine("return default;", _baseIndent);
        return builder.ToString();
    }
}