
using MediatR;
using RealTimeChat.Application.ViewModels;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAsync(request.Skip, request.Take);

        var data = users.Select(x => new GetUsersViewModel(x.Id, x.Username, x.Email, x.CreatedAt)).ToList();

        return PagedResult.SuccessResult(data, request.PageNumber, request.PageSize, data.Count, "Success!");

    }
}
