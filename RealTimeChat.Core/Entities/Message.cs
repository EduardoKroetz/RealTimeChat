
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Core.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    [ForeignKey("Sender")]
    public int SenderId { get; set; }

    public User Sender { get; set; }

    [ForeignKey("Room")]
    public int RoomId { get; set; }

    public ChatRoom Room { get; set; }
}