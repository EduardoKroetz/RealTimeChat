using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetUser;

public class GetUserQuery : IRequest<Result>
{
    public Guid UserId { get; set; }
}
