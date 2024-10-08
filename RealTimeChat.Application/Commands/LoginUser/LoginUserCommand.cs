﻿using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<ResultDTO>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
