using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace APIWeaver.Demo.GeneratedClient;

public sealed class UserClientExample(HttpClient httpClient)
{
    public async Task<Response<TOk, TNotFound>> GetUserAotAsync<TOk, TNotFound>(Guid id, int age, string? name, JsonTypeInfo<TOk> typeInfoOk, JsonTypeInfo<TNotFound> typeInfoNotFound)
    {
        using var request = new HttpRequestMessage();
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri($"/v1/users/{id}", UriKind.Relative);

        request.Headers.Add("age", age.ToString());
        if (name is not null)
        {
            request.Headers.Add("name", name);
        }

        using var httpResponse = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        await using var stream = await httpResponse.Content.ReadAsStreamAsync();

        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
            {
                var content = await JsonSerializer.DeserializeAsync(stream, typeInfoOk);

                return new Response<TOk, TNotFound>
                {
                    StatusCode = httpResponse.StatusCode,
                    Ok = content
                };
            }
            case HttpStatusCode.NotFound:
            {
                var content = await JsonSerializer.DeserializeAsync(stream, typeInfoNotFound);

                return new Response<TOk, TNotFound>
                {
                    StatusCode = httpResponse.StatusCode,
                    NotFound = content
                };
            }
        }

        return new Response<TOk, TNotFound>
        {
            StatusCode = httpResponse.StatusCode,
            Error = new Error("Unexpected status code")
        };
    }


    // public async Task<Response<TOk, TNotFound>> GetUserAsync<TOk, TNotFound>(Guid id, int age, string? name)
    // {
    //     using var request = new HttpRequestMessage();
    //
    //     request.Method = HttpMethod.Post;
    //     request.RequestUri = new Uri($"/v1/users/{id}", UriKind.Relative);
    //
    //     request.Headers.Add("age", age.ToString());
    //     if (name is not null)
    //     {
    //         request.Headers.Add("name", name);
    //     }
    //
    //     using var httpResponse = await httpClient.SendAsync(request);
    //     await using var stream = await httpResponse.Content.ReadAsStreamAsync();
    //
    //     switch (httpResponse.StatusCode)
    //     {
    //         case HttpStatusCode.OK:
    //         {
    //             var content = await JsonSerializer.DeserializeAsync<TOk>(stream);
    //
    //             return new Response<TOk, TNotFound>
    //             {
    //                 StatusCode = httpResponse.StatusCode,
    //                 Ok = content
    //             };
    //         }
    //         case HttpStatusCode.NotFound:
    //         {
    //             var content = await JsonSerializer.DeserializeAsync<TNotFound>(stream);
    //
    //             return new Response<TOk, TNotFound>
    //             {
    //                 StatusCode = httpResponse.StatusCode,
    //                 NotFound = content
    //             };
    //         }
    //     }
    //
    //     return new Response<TOk, TNotFound>
    //     {
    //         StatusCode = httpResponse.StatusCode,
    //         Error = new Error("Unexpected status code")
    //     };
    // }


    public async Task<Response<TOk, TNotFound>> PostUserAotAsync<TOk, TNotFound, TBody>(TBody body, JsonTypeInfo<TOk> typeInfoOk, JsonTypeInfo<TNotFound> typeInfoNotFound, JsonTypeInfo<TBody> typeInfoBody)
    {
        using var request = new HttpRequestMessage();
        request.Method = HttpMethod.Post;

        var jsonContent = JsonContent.Create(body, typeInfoBody);
        request.Content = jsonContent;

        request.RequestUri = new Uri("/v1/users/", UriKind.Relative);

        using var httpResponse = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        await using var stream = await httpResponse.Content.ReadAsStreamAsync();

        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
            {
                var responseContent = await JsonSerializer.DeserializeAsync(stream, typeInfoOk);
                return new Response<TOk, TNotFound>
                {
                    StatusCode = httpResponse.StatusCode,
                    Ok = responseContent
                };
            }
            case HttpStatusCode.NotFound:
            {
                var content = await JsonSerializer.DeserializeAsync(stream, typeInfoNotFound);

                return new Response<TOk, TNotFound>
                {
                    StatusCode = httpResponse.StatusCode,
                    NotFound = content
                };
            }
        }

        return new Response<TOk, TNotFound>
        {
            StatusCode = httpResponse.StatusCode,
            Error = new Error("Unexpected status code")
        };
    }
}

public sealed class ProblemDetails
{
    public string? Type { get; init; }
    public string? Title { get; init; }
    public string? Detail { get; init; }
    public string? Instance { get; init; }
    public int? Status { get; init; }
}

// public readonly struct Response<TOk, TNotFound>
// {
//     [MemberNotNullWhen(true, nameof(Ok))]
//     public bool IsSuccess => (int) StatusCode is >= 200 and < 300;
//
//     public required HttpStatusCode StatusCode { get; init; }
//
//     public TOk? Ok { get; init; }
//
//     public TNotFound? NotFound { get; init; }
//
//     public Error? Error { get; init; }
// }

public sealed record Error(string Message, Exception? Exception = null);