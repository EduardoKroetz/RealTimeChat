using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RealTimeChat.Core.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [MaxLength(256)]
    public string PasswordHash { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ICollection<Message> SentMessages { get; set; } = [];
    [JsonIgnore]
    public ICollection<RoomParticipant> RoomParticipants { get; set; } = [];
}