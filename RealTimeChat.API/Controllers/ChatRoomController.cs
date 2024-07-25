using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Commands.CreateChatRoom;

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

}
