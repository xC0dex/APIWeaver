using System.Net;
using APIWeaver.Generators.CSharp;

namespace APIWeaver;

internal sealed class CSharpClientProcessor(IOptions<GeneratorConfiguration> options, ResponseCache responseCache)
{
    public IEnumerable<CSharpFile> PrepareClient(OpenApiDocument document)
    {
        var operationsByTag = document.GetOperationsByTag();

        foreach (var (operationTag, openApiOperations) in operationsByTag)
        {
            var clientName = options.Value.NamePattern.Replace("{tag}", operationTag);
            var methods = PrepareMethods(openApiOperations);
            List<Parameter> constructorParameters =
            [
                new()
                {
                    Name = "httpClient",
                    Type = "HttpClient",
                    Nullable = false
                }
            ];
            var apiClient = new Class
            {
                AccessModifier = AccessModifier.Public,
                Type = ClassType.SealedClass,
                ConstructorParameters = constructorParameters,
                Name = clientName,
                Methods = methods
            };

            var file = new CSharpFile
            {
                PreProcessorDirectives = ["nullable enable"],
                Name = clientName,
                Usings = ["System.Text.Json.Serialization.Metadata"],
                Namespace = options.Value.Namespace,
                Classes = [apiClient]
            };
            yield return file;
        }

        var responseTypes = responseCache.GetUniqueCombinations();
        var classes = PrepareResponseClasses(responseTypes);
        var responseTypeFile = new CSharpFile
        {
            PreProcessorDirectives = ["nullable enable"],
            Name = "Response",
            Usings = ["System.Net"],
            Namespace = options.Value.Namespace,
            Classes = classes
        };
        yield return responseTypeFile;
    }

    private static List<Class> PrepareResponseClasses(IEnumerable<ResponseType[]> responseTypes)
    {
        var classes = responseTypes.Select(responseType =>
        {
            var responseTypeParameters = responseType.Select(type => new TypeParameter
            {
                Name = $"T{type.Name}"
            }).ToList();

            var properties = new List<Property>
            {
                new()
                {
                    AccessModifier = AccessModifier.Public,
                    Type = "bool",
                    Name = "IsSuccess",
                    Required = false,
                    Nullable = false,
                    ExpressionBody = "(int) StatusCode is >= 200 and <= 299"
                },
                new()
                {
                    AccessModifier = AccessModifier.Public,
                    Type = nameof(HttpStatusCode),
                    Name = "StatusCode",
                    Required = true,
                    Nullable = false
                }
            };
            properties.AddRange(responseType.Select(type => new Property
            {
                Name = type.Name,
                Type = $"T{type.Name}",
                Nullable = true,
                AccessModifier = AccessModifier.Public,
                Required = false
            }));
            properties.Add(new Property
            {
                AccessModifier = AccessModifier.Public,
                Type = nameof(Stream),
                Name = "BodyStream",
                Required = false,
                Nullable = true
            });


            return new Class
            {
                AccessModifier = AccessModifier.Public,
                Type = ClassType.ReadonlyStruct,
                Name = "Response",
                TypeParameters = responseTypeParameters,
                Properties = properties
            };
        }).ToList();
        return classes;
    }

    private List<Method> PrepareMethods(List<KeyValuePair<OperationType, OpenApiOperation>> operations)
    {
        var methods = new List<Method>();

        foreach (var operation in operations)
        {
            var responseTypes = GetResponseTypes(operation.Value).ToArray();
            responseCache.Add(responseTypes);
            // TODO: Get header, query and route values
            // Pass it to PrepareMethodParameters
            var parameters = PrepareMethodParameters(responseTypes);
            var method = new Method
            {
                AccessModifier = AccessModifier.Public,
                Name = GetMethodName(operation.Value),
                ResponseTypes = responseTypes,
                BodyFunc = builder =>
                {
                    // TODO: Pass all required OpenApi Information to the builder like the prepared header values, etc
                    new ApiClientMethodBodyGenerator().Generate(builder);
                },
                Parameters = parameters
            };
            methods.Add(method);
        }

        return methods;
    }

    private List<Parameter> PrepareMethodParameters(ResponseType[] responseTypes)
    {
        var parameters = new List<Parameter>();
        foreach (var responseType in responseTypes)
        {
            var parameter = new Parameter
            {
                Name = $"typeInfo{responseType.Name}",
                Type = $"JsonTypeInfo<T{responseType.Name}>",
                Nullable = false
            };
            parameters.Add(parameter);
        }

        parameters.Add(new Parameter
        {
            Name = "cancellationToken",
            Type = "CancellationToken",
            Nullable = false,
            Default = "default"
        });

        return parameters;
    }

    private static string GetMethodName(OpenApiOperation operation)
    {
        var operationSpan = operation.OperationId.AsSpan();
        var index = operationSpan.LastIndexOf('_');
        if (index != -1)
        {
            var slice = operationSpan[(index + 1)..];
            return $"{slice}Async";
        }

        return operation.OperationId;
    }

    private static IEnumerable<ResponseType> GetResponseTypes(OpenApiOperation operation)
    {
        var responseType = operation.Responses;
        var jsonResponses = responseType.Where(x => x.Value.Content.ContainsKey("application/json"));
        return jsonResponses.Select(x => new ResponseType
        {
            Name = x.Key.ToName(),
            StatusCode = int.Parse(x.Key)
        });
    }
}