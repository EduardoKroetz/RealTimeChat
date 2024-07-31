namespace RealTimeChat.Core.DTOs;

public class GetMessageUserDTO
{
    public GetMessageUserDTO(Guid userId, string username)
    {
        UserId = userId;
        Username = username;
    }

    public Guid UserId { get; set; }
    public string Username { get; set; }
}
