﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Commands.CreateChatRoom;
using RealTimeChat.Application.Commands.DeleteChatRoom;
using RealTimeChat.Application.Commands.UpdateChatRoom;
using RealTimeChat.Application.Queries.GetAllChatRooms;

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

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
        var query = new GetChatRoomsQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query);
        return Ok(result);
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

    [HttpPut]
    public async Task<IActionResult> UpdatedAsync([FromBody] UpdateChatRoomCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}
