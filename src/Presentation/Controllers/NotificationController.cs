namespace MsClean.Presentation.Controllers;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MsClean.Application;

public class NotificationController : BaseApiController
{
    public NotificationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(SendNotificationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
