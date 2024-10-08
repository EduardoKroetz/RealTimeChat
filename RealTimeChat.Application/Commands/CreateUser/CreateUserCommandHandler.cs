﻿using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repositories;
using RealTimeChat.Core.Services;

namespace RealTimeChat.Application.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResultDTO>
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public async Task<ResultDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User already registered");
        }

        var hashedPassword = _authService.HashPassword(request.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = hashedPassword,
        };

        await _userRepository.AddAsync(user);

        var token = _authService.GenerateJwtToken(user.Id, user.Email);
        return ResultDTO.SuccessResult(new { token, id = user.Id }, "User registered succesfully");
    }
}
