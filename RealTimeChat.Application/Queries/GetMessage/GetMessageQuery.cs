using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetMessage;

public class GetMessageQuery : IRequest<ResultDTO>
{
    public Guid MessageId { get; set; }
}
