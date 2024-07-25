namespace RealTimeChat.Application.ViewModels;

public class GetMessageViewModel
{
    public GetMessageViewModel(Guid id, string content, DateTime timestamp, Guid senderId, Guid chatRoomId)
    {
        Id = id;
        Content = content;
        Timestamp = timestamp;
        SenderId = senderId;
        ChatRoomId = chatRoomId;
    }

    public Guid Id { get; set; }

    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    public Guid SenderId { get; set; }

    public Guid ChatRoomId { get; set; }
}
