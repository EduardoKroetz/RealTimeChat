
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Core.Entities;

public class RoomParticipant
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public User User { get; set; }

    [ForeignKey("Room")]
    public Guid ChatRoomId { get; set; }

    public ChatRoom ChatRoom { get; set; }

}