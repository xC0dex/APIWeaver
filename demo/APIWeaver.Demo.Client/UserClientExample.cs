using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace APIWeaver.Demo.Client;

public sealed class UserClientExample(HttpClient httpClient)
{

    public async Task<Response<TOk, TNotFound>> GetUserAotAsync<TOk, TNotFound>(Guid id, int age, string? name, JsonTypeInfo<TOk> typeInfoOk, JsonTypeInfo<TNotFound> typeInfoNotFound)
    {
        var request = new HttpRequestMessage();

        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri($"/v1/users/{id}", UriKind.Relative);

        request.Headers.Add("age", age.ToString());
        if (name is not null)
        {
            request.Headers.Add("name", name);
        }

        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        await using var stream = await response.Content.ReadAsStreamAsync();

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            {
                var content = await JsonSerializer.DeserializeAsync(stream, typeInfoOk);

                return new Response<TOk, TNotFound>
                {
                    Ok = content
                };
            }
            case HttpStatusCode.NotFound:
            {
                var content = await JsonSerializer.DeserializeAsync(stream, typeInfoNotFound);

                return new Response<TOk, TNotFound>
                {
                    NotFound = content
                };
            }
        }

        return new Response<TOk, TNotFound>
        {
            Error = new Error("Unexpected status code")
        };
    }
    
    
    public async Task<Response<TOk, TNotFound>> GetUserAsync<TOk, TNotFound>(Guid id, int age, string? name)
    {
        var request = new HttpRequestMessage();

        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri($"/v1/users/{id}", UriKind.Relative);

        request.Headers.Add("age", age.ToString());
        if (name is not null)
        {
            request.Headers.Add("name", name);
        }

        using var response = await httpClient.SendAsync(request);
        await using var stream = await response.Content.ReadAsStreamAsync();

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            {
                var content = await JsonSerializer.DeserializeAsync<TOk>(stream);

                return new Response<TOk, TNotFound>
                {
                    Ok = content
                };
            }
            case HttpStatusCode.NotFound:
            {
                var content = await JsonSerializer.DeserializeAsync<TNotFound>(stream);

                return new Response<TOk, TNotFound>
                {
                    NotFound = content
                };
            }
        }

        return new Response<TOk, TNotFound>
        {
            Error = new Error("Unexpected status code")
        };
    }
}

public readonly struct Response<TOk, TNotFound>
{
    public TOk? Ok { get; init; }

    public TNotFound? NotFound { get; init; }

    public Error? Error { get; init; }
}

public sealed record Error(string Message, Exception? Exception = null);