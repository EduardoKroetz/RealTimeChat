using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Commands.DeleteChatRoom;

public class DeleteChatRoomCommand : IRequest<ResultDTO>
{
    public Guid ChatRoomId { get; set; }
}
