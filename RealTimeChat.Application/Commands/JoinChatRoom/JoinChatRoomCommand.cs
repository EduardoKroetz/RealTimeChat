﻿using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Commands.JoinChatRoom;

public class JoinChatRoomCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public Guid ChatRoomId { get; set; }
}