namespace APIWeaver;

internal sealed class IndentedStringBuilder(int indent)
{
    private const string IndentString = "    ";
    private readonly StringBuilder _builder = new();
    private int _currentIndent = indent;

    public void IncreaseIndent() => _currentIndent++;
    public void DecreaseIndent() => _currentIndent--;

    public void AppendIndent()
    {
        for (var i = 0; i < _currentIndent; i++) _builder.Append(IndentString);
    }

    public void AppendLine()
    {
        _builder.AppendLine();
    }

    public void AppendLine(string value)
    {
        AppendLine();
        AppendIndent();
        _builder.Append(value);
    }

    public void Append(string value)
    {
        _builder.Append(value);
    }

    public void Append(char value)
    {
        _builder.Append(value);
    }

    public override string ToString() => _builder.ToString();
}