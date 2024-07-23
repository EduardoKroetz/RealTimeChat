namespace RealTimeChat.Core.Services;

public interface IAuthService
{
    string GenerateJwtToken(string email);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
