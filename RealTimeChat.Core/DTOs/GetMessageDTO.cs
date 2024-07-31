namespace RealTimeChat.Core.DTOs;

public class GetMessageDTO
{
    public GetMessageDTO(Guid id, string content, DateTime timestamp, Guid senderId, Guid chatRoomId, GetMessageUserDTO sender)
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
    public GetMessageUserDTO Sender { get; set; }

    public Guid ChatRoomId { get; set; }
}
