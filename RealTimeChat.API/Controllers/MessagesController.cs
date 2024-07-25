using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Queries.GetMessages;

namespace RealTimeChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("chatrooms/{chatRoomId:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid chatRoomId ,[FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
        var query = new GetChatRoomMessagesQuery { ChatRoomId = chatRoomId, PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}
