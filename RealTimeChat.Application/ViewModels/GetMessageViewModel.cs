namespace RealTimeChat.Application.ViewModels;

public class GetMessageViewModel
{
    public GetMessageViewModel(Guid id, string content, DateTime timestamp, Guid senderId, Guid chatRoomId, GetMessageUser sender)
    {
        Id = id;
        Content = content;
        Timestamp = timestamp;
        SenderId = senderId;
        ChatRoomId = chatRoomId;
        Sender = sender;
    }

    public Guid Id { get; set; }

    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    public Guid SenderId { get; set; }
    public GetMessageUser Sender { get; set; }

    public Guid ChatRoomId { get; set; }
}
