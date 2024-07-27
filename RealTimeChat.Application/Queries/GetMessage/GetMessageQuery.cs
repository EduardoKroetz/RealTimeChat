using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetMessage;

public class GetMessageQuery : IRequest<Result>
{
    public Guid MessageId { get; set; }
}
