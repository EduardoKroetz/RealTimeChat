using MediatR;
using RealTimeChat.Core.DTOs;


namespace RealTimeChat.Application.Commands.UpdateChatRoom;

public class UpdateChatRoomCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
