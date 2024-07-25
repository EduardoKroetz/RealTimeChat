using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Commands.DeleteChatRoom;

public class DeleteChatRoomCommand : IRequest<Result>
{
    public Guid ChatRoomId { get; set; }
}
