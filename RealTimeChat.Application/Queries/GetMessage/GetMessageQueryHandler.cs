
using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetMessage;

public class GetMessageQueryHandler : IRequestHandler<GetMessageQuery, ResultDTO>
{
    private readonly IMessageRepository _messageRepository;

    public GetMessageQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<ResultDTO> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null)
        {
            throw new NotFoundException("Message not found");
        }

        var data = new GetMessageDTO(message.Id, message.Content, message.Timestamp, message.SenderId, message.ChatRoomId, new GetMessageUserDTO(message.Sender.Id,message.Sender.Username));

        return ResultDTO.SuccessResult(data, "Success!");   
    }
}
