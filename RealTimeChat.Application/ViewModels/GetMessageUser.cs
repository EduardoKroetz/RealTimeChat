namespace RealTimeChat.Application.ViewModels;

public class GetMessageUser
{
    public GetMessageUser(Guid userId, string username)
    {
        UserId = userId;
        Username = username;
    }

    public Guid UserId { get; set; }
    public string Username { get; set; }
}
