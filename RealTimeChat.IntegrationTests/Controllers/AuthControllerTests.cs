
using Newtonsoft.Json;
using RealTimeChat.Application.Commands.CreateUser;
using RealTimeChat.Application.Commands.LoginUser;
using RealTimeChat.IntegrationTests.Utils;
using System.Net;
using System.Net.Http.Json;

namespace RealTimeChat.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    private const string _baseUrl = "api/auth";

    public AuthControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnOk()
    {
        //Arrange
        var command = new CreateUserCommand { Username = "Pedro", Email = "pedro@gmail.com", Password = "123" };

        //Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/register", command);

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Result<DataToken>>(await response.Content.ReadAsStringAsync())!;

        Assert.True(content.data.token.Length > 50);
    }


    [Fact]
    public async Task RegisterUserAsync_WithUserAlreadyRegistered_ShouldReturnBadRequest()
    {
        //Arrange
        var command = new CreateUserCommand { Username = "Pedro", Email = "pedro@gmail.com", Password = "123" };

        //Act
        await _client.PostAsJsonAsync($"{_baseUrl}/register", command);

        var response = await _client.PostAsJsonAsync($"{_baseUrl}/register", command);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Fact]
    public async Task LoginUserAsync_ShouldReturnOk_WithValidCredentials()
    {
        // Arrange
        var registerCommand = new CreateUserCommand { Username = "Pedro", Email = "pedro@gmail.com", Password = "123" };
        await _client.PostAsJsonAsync($"{_baseUrl}/register", registerCommand);

        var loginCommand = new LoginUserCommand { Email = "pedro@gmail.com", Password = "123" };

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/login", loginCommand);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Result<DataToken>>(await response.Content.ReadAsStringAsync())!;

        Assert.NotNull(content.data);
        Assert.True(content.data.token.Length > 50); // Assuming the token is of significant length
        Assert.Equal("Login successfully", content.message);
    }

    [Fact]
    public async Task LoginUserAsync_WithInvalidCredentials_ShouldReturnBadRequest()
    {
        // Arrange
        var loginCommand = new LoginUserCommand { Email = "nonexistent@gmail.com", Password = "wrongpassword" };

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/login", loginCommand);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;
        Assert.Equal("Email or password are invalid", content.message);
    }
}
