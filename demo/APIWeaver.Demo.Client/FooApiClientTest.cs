namespace APIWeaver.Demo.Client;

public interface IFooApiTestClient;

// public class FooApiTestClient: IFooApiTestClient
// {
//
//     public async Task<UserResponse> GetUserAsync(UserRequest userRequest)
//     {
//         await Task.Delay(100);
//         return JsonSerializer.Deserialize<UserResponse>("");
//     }
//     
//     public async Task<TResponse> GetGenericUserAsync<TRequest, TResponse>(TRequest userRequest)
//     {
//         await Task.Delay(100);
//         var httpClient = new HttpClient();
//         
//         return JsonSerializer.Deserialize<TResponse>("");
//     }
//     
// }





public record UserResponse(string Name);
public record UserRequest(string Id);
