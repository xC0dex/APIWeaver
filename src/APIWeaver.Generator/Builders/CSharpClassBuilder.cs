namespace APIWeaver;

internal sealed class CSharpClassBuilder
{
    private readonly StringBuilder _builder = new();
    private int _indent;

    public string Build(Class classToBuild)
    {
        _builder.AppendIndent(classToBuild.AccessModifier.ToStringFast());
        _builder.Append(' ');
        _builder.Append(classToBuild.Type.ToStringFast());
        _builder.Append(' ');
        _builder.Append(classToBuild.Name);
        BuildTypeParameters(classToBuild);
        _builder.AppendLine();
        _builder.AppendLine("{");
        _indent++;
        BuildProperties(classToBuild.Properties);
        _builder.AppendIndentLine(_indent);

        _indent--;
        _builder.AppendLine("}");
        return _builder.ToString();
    }

    private void BuildTypeParameters(Class classToBuild)
    {
        if (classToBuild.TypeParameters.Count > 0)
        {
            _builder.Append('<');
            for (var i = 0; i < classToBuild.TypeParameters.Count; i++)
            {
                _builder.Append(classToBuild.TypeParameters[i].Name);
                if (i < classToBuild.TypeParameters.Count - 1)
                {
                    _builder.Append(", ");
                }
            }

            _builder.Append('>');
        }
    }

    private void BuildProperties(List<Property>? properties)
    {
        if (properties is null)
        {
            return;
        }

        foreach (var property in properties)
        {
            _builder.AppendIndent(property.AccessModifier.ToStringFast(), _indent);
            _builder.Append(' ');
            if (property.Required)
            {
                _builder.Append("required");
                _builder.Append(' ');
            }

            _builder.Append(property.Type);
            if (property.Nullable)
            {
                _builder.Append('?');
            }

            _builder.Append(' ');
            _builder.Append(property.Name);
            if (property.ExpressionBody is not null)
            {
                _builder.Append(" => ");
                _builder.Append(property.ExpressionBody);
                _builder.Append(';');
                _builder.AppendIndentLine();
                continue;
            }

            // TODO: REPLACE WITH ACTUAL GETTER SETTER
            _builder.Append(" { get; init; }");
            _builder.AppendIndentLine();
            if (property != properties[^1])
            {
                _builder.AppendIndentLine();
            }
        }
    }
}