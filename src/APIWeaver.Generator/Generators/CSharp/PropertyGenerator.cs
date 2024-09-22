namespace APIWeaver.Generators.CSharp;

internal sealed class PropertyGenerator
{
    private readonly IndentedStringBuilder _builder = new(1);

    public string Generate(List<Property> properties)
    {
        for (var i = 0; i < properties.Count; i++)
        {
            _builder.AppendIndent();
            Generate(properties[i]);
            _builder.AppendLine();
            if (i < properties.Count - 1)
            {
                _builder.AppendLine();
            }
        }

        return _builder.ToString();
    }

    private void Generate(Property property)
    {
        _builder.Append(property.AccessModifier.ToStringFast());
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
            return;
        }

        // TODO: REPLACE WITH ACTUAL GETTER SETTER
        _builder.Append(" { get; init; }");
    }
}