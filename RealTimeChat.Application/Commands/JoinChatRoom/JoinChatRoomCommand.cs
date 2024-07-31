using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Commands.JoinChatRoom;

public class JoinChatRoomCommand : IRequest<ResultDTO>
{
    public Guid UserId { get; set; }
    public Guid ChatRoomId { get; set; }
}
