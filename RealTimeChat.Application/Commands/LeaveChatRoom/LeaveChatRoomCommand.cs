﻿using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Commands.LeaveChatRoom;

public class LeaveChatRoomCommand : IRequest<ResultDTO>
{
    public Guid UserId { get; set; }
    public Guid ChatRoomId { get; set; }
}
