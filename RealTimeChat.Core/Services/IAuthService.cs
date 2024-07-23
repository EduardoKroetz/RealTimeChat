namespace RealTimeChat.Core.Services;

public interface IAuthService
{
    string GenerateJwtToken(string email, string role);
    string HashPassword(string password);
}
