using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using APIWeaver.ControllerApi.Demo.Controllers;

namespace APIWeaver.Demo.Controller;

public class UserClient(HttpClient httpClient)
{
    public async Task<UserDto?> GetUser1Async(Guid id, int age, string? name, JsonTypeInfo<UserDto> typeInfo)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"users/{id}");
        request.Headers.Add("age", age.ToString());
        if (name is not null)
        {
            request.Headers.Add("name", name);
        }

        var response = await httpClient.SendAsync(request);
        await using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync(stream, typeInfo);
    }

    public async Task<Response<TOk, TNotFound>> GetUserAsync<TOk, TNotFound>(Guid id, int age, string? name, JsonTypeInfo<TOk> typeInfo200, JsonTypeInfo<TNotFound> typeInfo404)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"users/{id}");

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
                var content = await JsonSerializer.DeserializeAsync(stream, typeInfo200);

                return new Response<TOk, TNotFound>
                {
                    Ok = content
                };
            }
            case HttpStatusCode.NotFound:
            {
                var content = await JsonSerializer.DeserializeAsync(stream, typeInfo404);

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