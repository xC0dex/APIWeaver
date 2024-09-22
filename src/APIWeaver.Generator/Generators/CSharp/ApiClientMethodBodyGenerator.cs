namespace APIWeaver.Generators.CSharp;

internal sealed class ApiClientMethodBodyGenerator
{
    public void Generate(IndentedStringBuilder builder)
    {
        builder.AppendLine("using var request = new HttpRequestMessage();");
        builder.AppendLine("await Task.CompletedTask;");
        builder.AppendLine("return default!;");
    }
}