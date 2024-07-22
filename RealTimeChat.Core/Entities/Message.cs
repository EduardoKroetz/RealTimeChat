
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Core.Entities;

public class Message
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    [ForeignKey("Sender")]
    public Guid SenderId { get; set; }

    public User Sender { get; set; }

    [ForeignKey("Room")]
    public Guid ChatRoomId { get; set; }

    public ChatRoom ChatRoom { get; set; }
}