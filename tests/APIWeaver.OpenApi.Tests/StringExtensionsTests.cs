namespace APIWeaver.OpenApi.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("GET", OperationType.Get)]
    [InlineData("PUT", OperationType.Put)]
    [InlineData("POST", OperationType.Post)]
    [InlineData("DELETE", OperationType.Delete)]
    [InlineData("OPTIONS", OperationType.Options)]
    [InlineData("HEAD", OperationType.Head)]
    [InlineData("PATCH", OperationType.Patch)]
    [InlineData("TRACE", OperationType.Trace)]
    public void ToOperationType_ShouldReturnOperationType_WhenStringContainsOperation(string httpMethod, OperationType expectedOperationType)
    {
        // Act
        var result = httpMethod.ToOperationType();

        // Assert
        result.Should().Be(expectedOperationType);
    }

    [Fact]
    public void ToOperationType_ShouldThrowException_WhenStringContainsInvalidHttpMethod()
    {
        // Arrange
        const string invalidHttpMethod = "INVALID";

        // Act
        Action act = () => invalidHttpMethod.ToOperationType();

        // Assert   
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Invalid http method type: {invalidHttpMethod}");
    }
}