using Microsoft.AspNetCore.Authorization;

namespace APIWeaver.OpenApi.Tests;

public class AuthorizationPolicyProviderExtensionsTests
{
    [Fact]
    public async Task HasFallbackPolicyAsync_ShouldReturnFalse_WhenProviderIsNull()
    {
        // Arrange
        IAuthorizationPolicyProvider? provider = null;

        // Act
        var result = await provider.HasFallbackPolicyAsync();

        // Arrange
        result.Should().BeFalse();
    }
}