using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Queries.GetAllChatRooms;

namespace RealTimeChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpHead("status")]
    public async Task<IActionResult> StatusAsync()
    {
        var query = new GetChatRoomsQuery { PageSize = 1, PageNumber = 1 };
        await _mediator.Send(query);
        return Ok("OK");
    }
}
