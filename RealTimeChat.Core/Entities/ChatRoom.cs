using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Core.Entities;

public class ChatRoom
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string RoomName { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("CreatedByUser")]
    public int CreatedBy { get; set; }

    public User CreatedByUser { get; set; }
}