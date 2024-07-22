
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Core.Entities;

public class RoomParticipant
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; }

    [ForeignKey("Room")]
    public int RoomId { get; set; }

    public ChatRoom Room { get; set; }

    public DateTime JoinedAt { get; set; }
}