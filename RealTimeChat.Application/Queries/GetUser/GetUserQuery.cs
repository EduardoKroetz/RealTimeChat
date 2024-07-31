using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetUser;

public class GetUserQuery : IRequest<ResultDTO>
{
    public Guid UserId { get; set; }
}
