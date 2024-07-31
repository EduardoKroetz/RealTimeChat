using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Commands.CreateUser;

public class CreateUserCommand : IRequest<ResultDTO>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
