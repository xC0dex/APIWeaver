namespace APIWeaver.Generators.CSharp;

internal sealed class ClassGenerator
{
    private readonly StringBuilder _builder = new();

    public string Generate(Class classToGenerate)
    {
        _builder.Append(classToGenerate.AccessModifier.ToStringFast());
        _builder.Append(' ');
        _builder.Append(classToGenerate.Type.ToStringFast());
        _builder.Append(' ');
        _builder.Append(classToGenerate.Name);
        BuildTypeParameters(classToGenerate);
        _builder.AppendLine();
        _builder.AppendLine("{");
        BuildProperties(classToGenerate.Properties);
        BuildMethods(classToGenerate.Methods);
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

        var code = new PropertyGenerator().Generate(properties);
        _builder.Append(code);
    }

    private void BuildMethods(List<Method>? methods)
    {
        if (methods is null)
        {
            return;
        }

        var code = new MethodGenerator().Generate(methods);
        _builder.Append(code);
    }
}