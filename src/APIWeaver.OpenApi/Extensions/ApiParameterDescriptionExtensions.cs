using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APIWeaver.OpenApi.Extensions;

internal static class ApiParameterDescriptionExtensions
{
    public static IEnumerable<ApiParameterDescription> GetPathParameters(this IEnumerable<ApiParameterDescription> parameterDescriptions)
    {
        return parameterDescriptions.Where(p => p.Source != BindingSource.Body && p.Source != BindingSource.Form && p.Source != BindingSource.FormFile);
    }

    public static IEnumerable<ApiParameterDescription> GetRequestBodyParameters(this IEnumerable<ApiParameterDescription> parameterDescriptions)
    {
        return parameterDescriptions.Where(p => p.Source == BindingSource.Body || p.Source == BindingSource.Form || p.Source == BindingSource.FormFile);
    }

    public static ParameterInfo GetParameterInfo(this ApiParameterDescription parameterDescriptor)
    {
        var descriptor = (parameterDescriptor.ParameterDescriptor as IParameterInfoParameterDescriptor)!;
        return descriptor.ParameterInfo;
    }
    
    public static ParameterLocation ToParameterLocation(this ApiParameterDescription parameterDescriptor)
    {
        var source = parameterDescriptor.Source;
        if (source == BindingSource.Path)
        {
            return ParameterLocation.Path;
        }

        return source == BindingSource.Header ? ParameterLocation.Header : ParameterLocation.Query;
    }
}