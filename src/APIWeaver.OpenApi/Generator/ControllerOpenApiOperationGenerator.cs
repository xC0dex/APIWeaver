using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace APIWeaver.OpenApi.Generator;

internal sealed class ControllerOpenApiOperationGenerator : IOpenApiOperationGenerator
{
    public OpenApiOperation GenerateOpenApiOperation(ApiDescription apiDescription) =>
        new()
        {
            Tags = GenerateTags(apiDescription.ActionDescriptor),
            Parameters = GenerateParameters(apiDescription)
        };

    private static List<OpenApiParameter> GenerateParameters(ApiDescription apiDescription)
    {
        var parameterDescriptions = apiDescription.ParameterDescriptions.GetPathParameters();
        return parameterDescriptions.Select(GenerateParameter).ToList();
    }
    
    private static OpenApiParameter GenerateParameter(ApiParameterDescription apiParameterDescription)
    {
        return new OpenApiParameter
        {
            Name = apiParameterDescription.Name,
            In = apiParameterDescription.ToParameterLocation(),
            Required = apiParameterDescription.IsRequired || apiParameterDescription.GetParameterInfo().GetCustomAttributes().OfType<RequiredAttribute>().Any()
        };
    }

    private static List<OpenApiTag> GenerateTags(ActionDescriptor actionDescriptor)
    {
        var tagsMetadata = actionDescriptor.EndpointMetadata.OfType<ITagsMetadata>().FirstOrDefault();
        if (tagsMetadata is not null)
        {
            return tagsMetadata.Tags.Select(name => new OpenApiTag
            {
                Name = name
            }).ToList();
        }

        var controllerName = actionDescriptor.RouteValues[RouteValuesConstants.Controller];
        return
        [
            new OpenApiTag
            {
                Name = controllerName
            }
        ];
    }
}