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