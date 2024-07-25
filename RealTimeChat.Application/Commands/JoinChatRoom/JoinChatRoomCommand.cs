using MediatR;
using RealTimeChat.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChat.Application.Commands.JoinChatRoom;

public class JoinChatRoomCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public Guid ChatRoomId { get; set; }
}
