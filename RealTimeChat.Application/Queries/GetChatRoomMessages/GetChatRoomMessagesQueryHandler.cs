using MediatR;
using RealTimeChat.Application.Queries.GetMessages;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetChatRoomMessages;

public class GetChatRoomMessagesQueryHandler : IRequestHandler<GetChatRoomMessagesQuery, PagedResultDTO>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetChatRoomMessagesQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<PagedResultDTO> Handle(GetChatRoomMessagesQuery request, CancellationToken cancellationToken)
    {
        var skip = ( request.PageNumber - 1 ) * request.PageSize;
        var messages = await _chatRoomRepository.GetMessagesInRoomAsync( skip, request.PageSize ,request.ChatRoomId);

        var result = messages.Select(x =>
            new GetMessageDTO(
                x.Id, 
                x.Content, 
                x.Timestamp, 
                x.SenderId, 
                x.ChatRoomId, 
                new GetMessageUserDTO(
                    x.Sender.Id, 
                    x.Sender.Username))
        ).ToList();

        return PagedResultDTO.SuccessResult(result, request.PageNumber, request.PageSize, result.Count, "Success!");
    }
}
