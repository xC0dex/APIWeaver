namespace APIWeaver.Generators.CSharp;

internal sealed class MethodGenerator
{
    private readonly IndentedStringBuilder _builder = new(1);

    public string Generate(List<Method> methods)
    {
        for (var i = 0; i < methods.Count; i++)
        {
            _builder.AppendIndent();
            Generate(methods[i]);
            _builder.AppendLine();
            if (i < methods.Count - 1)
            {
                _builder.AppendLine();
            }
        }

        return _builder.ToString();
    }

    private void Generate(Method method)
    {
        GenerateSignature(method);
        if (method.BodyFunc is null)
        {
            _builder.Append(';');
            return;
        }

        _builder.AppendLine("{");
        GenerateBody(method);
        _builder.AppendLine("}");
    }

    private void GenerateSignature(Method method)
    {
        _builder.Append(method.AccessModifier.ToStringFast());
        _builder.Append(' ');
        _builder.Append("async");
        _builder.Append(' ');
        BuildReturnType(method);
        _builder.Append(' ');
        _builder.Append($"{method.Name}");
        _builder.Append('<');
        for (var i = 0; i < method.ResponseTypes.Length; i++)
        {
            _builder.Append($"T{method.ResponseTypes[i].Name}");
            if (i < method.ResponseTypes.Length - 1)
            {
                _builder.Append(", ");
            }
        }

        _builder.Append('>');
        _builder.Append('(');
        for (var i = 0; i < method.Parameters.Count; i++)
        {
            _builder.Append($"{method.Parameters[i].Type}");
            if (method.Parameters[i].Nullable)
            {
                _builder.Append('?');
            }

            _builder.Append(' ');
            _builder.Append($"{method.Parameters[i].Name}");
            if (method.Parameters[i].Default is not null)
            {
                _builder.Append(" = ");
                _builder.Append(method.Parameters[i].Default!);
            }

            if (i < method.Parameters.Count - 1)
            {
                _builder.Append(", ");
            }
        }

        _builder.Append(')');
    }

    private void BuildReturnType(Method method)
    {
        _builder.Append("Task");
        _builder.Append('<');
        _builder.Append("Response");
        _builder.Append('<');
        for (var i = 0; i < method.ResponseTypes.Length; i++)
        {
            _builder.Append($"T{method.ResponseTypes[i].Name}");
            if (i < method.ResponseTypes.Length - 1)
            {
                _builder.Append(", ");
            }
        }

        _builder.Append('>');
        _builder.Append('>');
    }

    private void GenerateBody(Method method)
    {
        _builder.IncreaseIndent();
        method.BodyFunc!.Invoke(_builder);
        _builder.DecreaseIndent();
    }
}