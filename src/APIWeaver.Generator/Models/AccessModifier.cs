using System.ComponentModel;
using NetEscapades.EnumGenerators;

namespace APIWeaver;

[EnumExtensions]
internal enum AccessModifier
{
    [Description("public")]
    Public,

    [Description("internal")]
    Internal
}

internal sealed record ResponseType
{
    public required string Name { get; init; }

    public required int StatusCode { get; init; }
}