using RealTimeChat.Core.Entities;

namespace RealTimeChat.Application.ViewModels;

public class GetChatRoomsViewModel
{
    public GetChatRoomsViewModel(Guid id, string name, DateTime createdAt, Guid createdBy)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
}
