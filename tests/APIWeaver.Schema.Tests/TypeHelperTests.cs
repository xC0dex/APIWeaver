namespace APIWeaver.Schema.Tests;

public class TypeHelperTests
{
    [Fact]
    public void PrimitiveTypes_ShouldContainExpectedDefinitions()
    {
        // Assert
        TypeHelper.PrimitiveTypes[typeof(Guid)].Format.Should().Be("uuid");
        TypeHelper.PrimitiveTypes[typeof(byte)].Format.Should().Be("int32");
        TypeHelper.PrimitiveTypes[typeof(sbyte)].Format.Should().Be("int32");
        TypeHelper.PrimitiveTypes[typeof(byte[])].Format.Should().Be("byte");
        TypeHelper.PrimitiveTypes[typeof(short)].Format.Should().Be("int32");
        TypeHelper.PrimitiveTypes[typeof(ushort)].Format.Should().Be("int32");
        TypeHelper.PrimitiveTypes[typeof(int)].Format.Should().Be("int32");
        TypeHelper.PrimitiveTypes[typeof(uint)].Format.Should().Be("int32");
        TypeHelper.PrimitiveTypes[typeof(long)].Format.Should().Be("int64");
        TypeHelper.PrimitiveTypes[typeof(ulong)].Format.Should().Be("int64");
        TypeHelper.PrimitiveTypes[typeof(float)].Format.Should().Be("float");
        TypeHelper.PrimitiveTypes[typeof(double)].Format.Should().Be("double");
        TypeHelper.PrimitiveTypes[typeof(decimal)].Format.Should().Be("double");
        TypeHelper.PrimitiveTypes[typeof(bool)].Format.Should().BeNull();
        TypeHelper.PrimitiveTypes[typeof(string)].Format.Should().BeNull();
        TypeHelper.PrimitiveTypes[typeof(char)].Format.Should().BeNull();
        TypeHelper.PrimitiveTypes[typeof(Uri)].Format.Should().Be("uri");
        TypeHelper.PrimitiveTypes[typeof(DateTime)].Format.Should().Be("date-time");
        TypeHelper.PrimitiveTypes[typeof(DateTimeOffset)].Format.Should().Be("date-time");
        TypeHelper.PrimitiveTypes[typeof(DateOnly)].Format.Should().Be("date");
        TypeHelper.PrimitiveTypes[typeof(TimeSpan)].Format.Should().Be("time");
    }
}