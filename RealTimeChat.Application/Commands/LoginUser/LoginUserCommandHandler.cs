using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Repositories;
using RealTimeChat.Core.Services;

namespace RealTimeChat.Application.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ResultDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<ResultDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            throw new ArgumentException("Email or password are invalid");
        }

        var validPassword = _authService.VerifyPassword(request.Password, user.PasswordHash);
        if (validPassword == false)
        {
            throw new ArgumentException("Email or password are invalid");
        }

        var token = _authService.GenerateJwtToken(user.Id ,request.Email);

        return ResultDTO.SuccessResult(new { token }, "Login successfully");
    }
}
