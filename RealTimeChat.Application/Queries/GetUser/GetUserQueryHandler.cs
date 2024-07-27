﻿using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserIncludesRoomParticipants(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return Result.SuccessResult(user, "Success!");
    }
}