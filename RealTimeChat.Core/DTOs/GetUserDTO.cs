﻿
namespace RealTimeChat.Core.DTOs;

public class GetUserDTO
{
    public GetUserDTO(Guid id, string username, string email, DateTime createdAt)
    {
        Id = id;
        Username = username;
        Email = email;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public DateTime CreatedAt { get; set; }
}
