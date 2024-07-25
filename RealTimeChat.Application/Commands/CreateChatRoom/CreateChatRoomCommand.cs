using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Commands.CreateChatRoom;

public class CreateChatRoomCommand : IRequest<Result>
{
    public string Name { get; set; }
    public Guid UserId { get; set; }
}