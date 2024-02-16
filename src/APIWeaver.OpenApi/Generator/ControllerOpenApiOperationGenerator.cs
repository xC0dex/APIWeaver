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
            Parameters = GenerateParameters(apiDescription),
            RequestBody = GenerateRequestBody(apiDescription),
            Responses = GenerateResponses(apiDescription)
        };

    private static OpenApiResponses GenerateResponses(ApiDescription apiDescription)
    {
        var apiResponseTypes = apiDescription.SupportedResponseTypes.DefaultIfEmpty(new ApiResponseType
        {
            StatusCode = StatusCodes.Status200OK
        });

        var openApiResponses = new OpenApiResponses();
        foreach (var apiResponseType in apiResponseTypes)
        {
            var response = new OpenApiResponse
            {
                Description = apiResponseType.StatusCode.GetDescription(),
                Content = apiResponseType.ApiResponseFormats.ToDictionary(contentType => contentType.MediaType, _ => new OpenApiMediaType()) // Todo: Get produce attribute data form endpointmetadata
            };
            openApiResponses.Add(apiResponseType.StatusCode.ToString(), response);
        }

        return openApiResponses;
    }


    private static OpenApiRequestBody? GenerateRequestBody(ApiDescription apiDescription)
    {
        var parameterDescription = apiDescription.ParameterDescriptions.GetRequestBodyParameters().FirstOrDefault();
        var mediaType = apiDescription.SupportedRequestFormats.FirstOrDefault()?.MediaType;
        if (parameterDescription is not null && mediaType is not null)
        {
            return new OpenApiRequestBody
            {
                Required = parameterDescription.IsRequired,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {mediaType, new OpenApiMediaType()}
                }
            };
        }

        return null;
    }

    private static List<OpenApiParameter> GenerateParameters(ApiDescription apiDescription)
    {
        var parameterDescriptions = apiDescription.ParameterDescriptions.GetPathParameters();
        return parameterDescriptions.Select(GenerateParameter).ToList();
    }

    private static OpenApiParameter GenerateParameter(ApiParameterDescription apiParameterDescription) =>
        new()
        {
            Name = apiParameterDescription.Name,
            In = apiParameterDescription.ToParameterLocation(),
            Required = apiParameterDescription.IsRequired || apiParameterDescription.GetParameterInfo().GetCustomAttributes().OfType<RequiredAttribute>().Any()
        };

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