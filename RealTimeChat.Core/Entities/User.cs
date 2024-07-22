using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Core.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

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
    public DateTime? LastLogin { get; set; }
}