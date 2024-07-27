
using MediatR;
using RealTimeChat.Application.ViewModels;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetMessage;

public class GetMessageQueryHandler : IRequestHandler<GetMessageQuery, Result>
{
    private readonly IMessageRepository _messageRepository;

    public GetMessageQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Result> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null)
        {
            throw new NotFoundException("Message not found");
        }

        var data = new GetMessageViewModel(message.Id, message.Content, message.Timestamp, message.SenderId, message.ChatRoomId);

        return Result.SuccessResult(data, "Success!");   
    }
}
