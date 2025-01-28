using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestTask.Application.CardActions.Queries;

namespace TestTask.Controllers;

[ApiController]
public class CardsController(IMediator mediator) : Controller
{
    [HttpPost]
    [Route("api/cards")]
    public async Task<IActionResult> GetCardDetails([FromBody] CardActionsQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}