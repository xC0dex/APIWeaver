using System.ComponentModel;
using NetEscapades.EnumGenerators;

namespace APIWeaver;

[EnumExtensions]
internal enum ClassType
{
    [Description("class")]
    Class,

    [Description("sealed class")]
    SealedClass,

    [Description("struct")]
    Struct,

    [Description("readonly struct")]
    ReadonlyStruct,

    [Description("interface")]
    Interface
}