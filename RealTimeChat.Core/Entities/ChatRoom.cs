using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Core.Entities;

public class ChatRoom
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("CreatedByUser")]
    public Guid CreatedBy { get; set; }

    public User CreatedByUser { get; set; }

    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<RoomParticipant> RoomParticipants { get; set; } = [];
}