using MediatR;
using RealTimeChat.Application.Queries.GetMessages;
using RealTimeChat.Application.ViewModels;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetChatRoomMessages;

public class GetChatRoomMessagesQueryHandler : IRequestHandler<GetChatRoomMessagesQuery, PagedResult>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetChatRoomMessagesQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<PagedResult> Handle(GetChatRoomMessagesQuery request, CancellationToken cancellationToken)
    {
        var skip = ( request.PageNumber - 1 ) * request.PageSize;
        var messages = await _chatRoomRepository.GetMessagesInRoomAsync( skip, request.PageSize ,request.ChatRoomId);

        var result = messages.Select(x =>
            new GetMessageViewModel(
                x.Id, 
                x.Content, 
                x.Timestamp, 
                x.SenderId, 
                x.ChatRoomId, 
                new GetMessageUser(
                    x.Sender.Id, 
                    x.Sender.Username))
        ).ToList();

        return PagedResult.SuccessResult(result, request.PageNumber, request.PageSize, result.Count, "Success!");
    }
}
