using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace APIWeaver.OpenApi.Tests;

public class ApiDescriptionExtensionsTests
{
    [Theory]
    [InlineData("api/v1/users/{userId}", "api/v1/users/{userId}")]
    [InlineData("api/v1/users/{userId}/{group}", "api/v1/users/{userId}/{group}")]
    [InlineData("api/v1/users/{userId:int}/{group}", "api/v1/users/{userId}/{group}")]
    [InlineData("api/v1/users/{userId:customType}/{group:guid}", "api/v1/users/{userId}/{group}")]
    public void GetRelativePath_ShouldReturnExpected(string relativePath, string expected)
    {
        // Arrange
        var apiDescription = new ApiDescription
        {
            RelativePath = relativePath
        };

        // Act
        var result = apiDescription.GetRelativePath();

        // Assert
        result.Should().Be(expected);
    }
}