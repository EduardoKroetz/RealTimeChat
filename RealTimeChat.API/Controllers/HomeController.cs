using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Queries.GetUser;

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
        var command = new GetUserQuery { UserId = Guid.NewGuid() };
        await _mediator.Send(command);
        return Ok("OK");
    }
}
