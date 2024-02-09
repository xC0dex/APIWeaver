using System.Collections;

namespace APIWeaver.Schema.Tests;

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(IEnumerable<>), true)]
    [InlineData(typeof(IEnumerable), true)]
    [InlineData(typeof(List<>), true)]
    [InlineData(typeof(string[]), true)]
    [InlineData(typeof(Dictionary<,>), true)]
    [InlineData(typeof(IAsyncEnumerable<>), true)]
    [InlineData(typeof(int), false)]
    public void IsEnumerable_ShouldReturnExpectedResult(Type type, bool expected)
    {
        var result = type.IsEnumerable();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(typeof(ISet<>), true)]
    [InlineData(typeof(HashSet<>), true)]
    [InlineData(typeof(List<>), false)]
    [InlineData(typeof(int), false)]
    public void IsSet_ShouldReturnExpectedResult(Type type, bool expected)
    {
        var result = type.IsSet();
        result.Should().Be(expected);
    }
}