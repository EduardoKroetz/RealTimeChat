
using Newtonsoft.Json;
using RealTimeChat.Application.Commands.CreateUser;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RealTimeChat.IntegrationTests.Utils;

public static partial class LoginUser
{ 
    public static async Task<Guid> Login(HttpClient client, CreateUserCommand? createUser = null)
    {
        var command = createUser ?? new CreateUserCommand { Username = "Pedro", Email = "pedro@gmail.com", Password = "123" };
        var response = await client.PostAsJsonAsync("api/auth/register", command);
        var content = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await response.Content.ReadAsStringAsync())!;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.data.token);
        return content.data.id;
    }
}