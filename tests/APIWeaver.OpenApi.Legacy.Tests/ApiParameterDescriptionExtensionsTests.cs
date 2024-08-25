using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APIWeaver.OpenApi.Tests;

public class ApiParameterDescriptionExtensionsTests
{
    public static TheoryData<BindingSource, ParameterLocation> BindingSourceData =>
        new()
        {
            {BindingSource.Path, ParameterLocation.Path},
            {BindingSource.Header, ParameterLocation.Header},
            {BindingSource.Query, ParameterLocation.Query},
            {BindingSource.Form, ParameterLocation.Query},
            {BindingSource.FormFile, ParameterLocation.Query},
            {BindingSource.Body, ParameterLocation.Query}
        };

    [Fact]
    public void GetPathParameters_ShouldOnlyReturnNonBodyParameters()
    {
        // Arrange
        ApiParameterDescription[] parameters =
        [
            CreateParameter(BindingSource.Path),
            CreateParameter(BindingSource.Body),
            CreateParameter(BindingSource.Query),
            CreateParameter(BindingSource.FormFile),
            CreateParameter(BindingSource.Form)
        ];

        // Act
        var result = parameters.GetPathParameters();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public void GetRequestBodyParameters_ShouldOnlyReturnNonBodyParameters()
    {
        // Arrange
        ApiParameterDescription[] parameters =
        [
            CreateParameter(BindingSource.Path),
            CreateParameter(BindingSource.Body),
            CreateParameter(BindingSource.Query),
            CreateParameter(BindingSource.FormFile),
            CreateParameter(BindingSource.Form)
        ];

        // Act
        var result = parameters.GetRequestBodyParameters();

        // Assert
        result.Should().HaveCount(3);
    }

    private static ApiParameterDescription CreateParameter(BindingSource source) =>
        new()
        {
            Source = source
        };

    [Fact]
    public void GetParameterInfo_ShouldReturnParameterInfo()
    {
        // Arrange
        var methodInfo = typeof(DummyController).GetMethod(nameof(DummyController.DummyAction));
        var parameterInfo = methodInfo!.GetParameters()[0];
        var parameterDescriptor = new ParameterDescriptor
        {
            ParameterType = typeof(string),
            Name = "testParam"
        };
        var apiParameterDescription = new ApiParameterDescription
        {
            ParameterDescriptor = new ControllerParameterDescriptor
            {
                ParameterInfo = parameterInfo,
                ParameterType = parameterDescriptor.ParameterType,
                Name = parameterDescriptor.Name
            }
        };

        // Act
        var result = apiParameterDescription.GetParameterInfo();

        // Assert
        result.Should().Be(parameterInfo);
    }

    [Theory]
    [MemberData(nameof(BindingSourceData))]
    public void ToParameterLocation_ShouldReturnExpectedLocation(BindingSource source, ParameterLocation expectedLocation)
    {
        // Arrange
        var parameterDescriptor = new ApiParameterDescription {Source = source};

        // Act
        var actualLocation = parameterDescriptor.ToParameterLocation();

        // Assert
        actualLocation.Should().Be(expectedLocation);
    }
}

file class DummyController
{
    public static void DummyAction(string testParam)
    {
    }
}