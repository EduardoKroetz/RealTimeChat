namespace RealTimeChat.Core.Services;

public interface IAuthService
{
    string GenerateJwtToken(Guid userId, string email);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
