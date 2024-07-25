using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Commands.CreateChatRoom;
using RealTimeChat.Application.Commands.DeleteChatRoom;

namespace RealTimeChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatRoomController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatRoomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateChatRoomCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeleteChatRoomCommand { ChatRoomId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}
